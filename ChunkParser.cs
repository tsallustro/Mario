using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Controllers;
using Commands;
using GameObjects;
using Factories;
using States;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using Sprites;
using Cameras;
using Chunks;

namespace ChunkReader
{
    class ChunkParser
    {
        private XElement level;
        private GraphicsDeviceManager graphics;
        private Point maxCoords;
        private Camera camera;
        private Texture2D blockSprites;
        private Texture2D pipeSprite;
        private Texture2D itemSprites;
        private int baseHeight;
        private Dictionary<int, Chunk> chunkMap;
        private Dictionary<int, List<int>> compatibleChunks;
        private int numberOfChunks;

        Mario mario;

        public ChunkParser(string levelPath, GraphicsDeviceManager graphics, Point maxCoords, Camera camera, Texture2D blockSprites, Texture2D pipeSprite, Texture2D itemSprites, int baseHeight)
        {
            level = XElement.Load(levelPath);
            this.graphics = graphics;
            this.maxCoords = maxCoords;
            this.camera = camera;
            this.blockSprites = blockSprites;
            this.pipeSprite = pipeSprite;
            this.itemSprites = itemSprites;
            this.baseHeight = baseHeight;
            chunkMap = new Dictionary<int, Chunk>();
            compatibleChunks = new Dictionary<int, List<int>>();
            numberOfChunks = 0;
        }

        public void ParseAllChunksAndAddToDictionary()
        {
            List<IGameObject> objects = new List<IGameObject>();
            IEnumerable<XElement> chunks = level.Element("chunks").Elements();
            List<XElement> allBlocks = new List<XElement>();

            // Must iterate over chunks to find chunk with correct ID
            foreach (XElement chunk in chunks)
            {
                if (chunk.HasAttributes)
                {
                    XAttribute id = chunk.Attribute("id");
                    
                    ParseWarpPipes(chunk, objects);
                    ParseFloorBlocks(chunk, objects);
                    allBlocks.AddRange(ParseBrickBlocks(chunk, objects));
                    allBlocks.AddRange(ParseQuestionBlocks(chunk, objects));
                    ParseHiddenBlocks(chunk, objects);
                    ParseItems(chunk, objects);
                    allBlocks.AddRange(ParseStairBlocks(chunk, objects));
                    ParseEnemies(chunk, objects);
                    ParseFireballs(objects);

                    // After this method, chunkMap will contain all possible chunks
                    chunkMap.Add(int.Parse(id.Value), new Chunk(objects, GetHighRows(allBlocks), GetLowRows(allBlocks)));
                    numberOfChunks++;
                }
            }
        }

        private bool GapIsAboveCurrentBlockInCurrentChunk(int[,] currentChunkHighRows, int row, int column)
        {
            bool blockHasGapAbove = true;
            bool blockHasGapOnLeft = true;
            bool blockHasGapOnRight = true;

            // Check if there is a gap in current chunk above the current block to jump
            for (int rowsAbove = 0; rowsAbove < row; rowsAbove++)
            {
                if (currentChunkHighRows[rowsAbove, column] == 1)
                    blockHasGapAbove = false;
            }

            // If blockHasGapAbove is still true here, there is 1-block gap directly above block
            // Now, we check if there is at least a 2-block gap to fit through
            for (int rowsAbove = 0; rowsAbove < row; rowsAbove++)
            {
                if (column < 49 && currentChunkHighRows[rowsAbove, column + 1] == 1)
                    blockHasGapOnRight = false;

                if (column > 0 && currentChunkHighRows[rowsAbove, column - 1] == 1)
                    blockHasGapOnLeft = false;
            }

            return blockHasGapAbove && (blockHasGapOnLeft || blockHasGapOnRight);
        }

        private bool NextChunkIsCompatible(int nextChunkIndex, int row, int column)
        {
            Chunk nextChunk = chunkMap[nextChunkIndex];
            int[,] nextChunkLowRows = nextChunk.GetLowRows();
            bool isCompatibleWithThisChunk = false;

            for (int nextRow = 0; nextRow < 7; nextRow++)
            {
                if (nextRow >= (row + 3)) // If next row is reachable from current row
                {
                    int heightDifference = 7 + row - nextRow;
                    int maximumDistance = 14 - (2 * heightDifference);

                    for (int nextColumn = 0; nextColumn < 50; nextColumn++)
                    {
                        int currentBlockInNextChunk = nextChunkLowRows[nextRow, nextColumn]; // 0 if no block, 1 if block

                        // If there is a block, check that the two columns above are clear for landing
                        // Also check that block is within jumping distance
                        // Don't need to check that nextRow >= 2 because it always will be here
                        if (currentBlockInNextChunk == 1 && 
                            nextChunkLowRows[nextRow - 1, nextColumn] == 0 &&
                            nextChunkLowRows[nextRow - 2, nextColumn] == 0 &&
                            Math.Abs(nextColumn - column) < maximumDistance)
                        {
                            // Check for walls between [row, column] and [nextRow, nextColumn]
                            int startingColumn;
                            int endingColumn;
                            bool wallIsBlockingPath = false;

                            // Determine start and end columns to ease loop logic
                            if (column > nextColumn)
                            {
                                startingColumn = nextColumn;
                                endingColumn = column;
                            }
                            else
                            {
                                startingColumn = column;
                                endingColumn = nextColumn;
                            }

                            // Check for walls between jumping point and ending point
                            for (int wallRow = nextRow - 1; wallRow > nextRow - 2; wallRow--)
                            {
                                for (int wallColumn = startingColumn; wallColumn < endingColumn; wallColumn++)
                                {
                                    if (nextChunkLowRows[wallRow, wallColumn] == 1)
                                    {
                                        wallIsBlockingPath = true;
                                    }
                                }
                            }

                            if (!wallIsBlockingPath) isCompatibleWithThisChunk = true;
                        }
                    }
                }
            }

            return isCompatibleWithThisChunk;
        }

        // This is gonna be ugly...
        public void DetermineCompatibleChunks()
        {
            for (int currentChunkIndex = 1; currentChunkIndex < numberOfChunks; currentChunkIndex++)
            {
                Chunk currentChunk = chunkMap[currentChunkIndex];
                int[,] currentChunkHighRows = currentChunk.GetHighRows();
                List<int> compatibleChunksForCurrentChunk = new List<int>();

                for (int row = 0; row < 5; row++)
                {
                    for (int column = 0; column < 50; column++)
                    {
                        if (currentChunkHighRows[row, column] == 1 && GapIsAboveCurrentBlockInCurrentChunk(currentChunkHighRows, row, column))
                        {
                            // Check if there is a block to land on in all other chunks
                            for (int nextChunkIndex = 1; nextChunkIndex < numberOfChunks; nextChunkIndex++)
                            {
                                if (nextChunkIndex != currentChunkIndex && NextChunkIsCompatible(nextChunkIndex, row, column))
                                    compatibleChunksForCurrentChunk.Add(nextChunkIndex);
                            }
                        }
                    }
                }

                compatibleChunks.Add(currentChunkIndex, compatibleChunksForCurrentChunk);
            }

            // Debug statements only, can safely erase
            for (int i = 1; i < numberOfChunks; i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    Debug.WriteLine("Compatible chunk for chunk " + i + ": " + compatibleChunks[i][j]);
                }
            }
        }

        public Chunk ParseChunk(int chunkId, int previousChunkId)
        {
            List<IGameObject> objects = new List<IGameObject>();
            IEnumerable<XElement> chunks = level.Element("chunks").Elements();
            List<XElement> allBlocks = new List<XElement>();

            // Must iterate over chunks to find chunk with correct ID
            foreach (XElement chunk in chunks)
            {
                if (chunk.HasAttributes)
                {
                    if (previousChunkId > 0 && !compatibleChunks[previousChunkId].Contains(chunkId))
                    {
                        Random rnd = new Random();

                        while (!compatibleChunks[previousChunkId].Contains(chunkId))
                        {
                            chunkId = rnd.Next(2, numberOfChunks);
                        }
                        // Need to get new chunk id since they're not compatible
                    }

                    XAttribute id = chunk.Attribute("id");
                    
                    if (id != null && int.Parse(id.Value) == chunkId)
                    {
                        ParseWarpPipes(chunk, objects);
                        ParseFloorBlocks(chunk, objects);
                        allBlocks.AddRange(ParseBrickBlocks(chunk, objects));
                        allBlocks.AddRange(ParseQuestionBlocks(chunk, objects));
                        ParseHiddenBlocks(chunk, objects);
                        ParseItems(chunk, objects);
                        allBlocks.AddRange(ParseStairBlocks(chunk, objects));
                        ParseEnemies(chunk, objects);
                        ParseFireballs(objects);
                    }
                }
            }

            baseHeight -= 480; // Decrement base height for each added chunk

            return new Chunk(objects, GetHighRows(allBlocks), GetLowRows(allBlocks));
        }

        public int[,] GetHighRows(List<XElement> blocks)
        {
            int[,] highRows = new int[5, 50];

            foreach (XElement block in blocks)
            {
                int Y = int.Parse(block.Element("row").Value);

                if (Y >= 0 && Y < 5)
                {
                    int X = int.Parse(block.Element("column").Value);
                    if (X >= 0 && X < 49) highRows[Y, X] = 1;
                }
            }

            return highRows;
        }

        public int[,] GetLowRows(List<XElement> blocks)
        {
            int[,] lowRows = new int[7, 50];

            foreach (XElement block in blocks)
            {
                int Y = int.Parse(block.Element("row").Value);

                if (Y >= 23 && Y < 30)
                {
                    int X = int.Parse(block.Element("column").Value);
                    Y -= 23; // Ensure Y is in range of array
                    if (X >= 0 && X < 49) lowRows[Y, X] = 1;
                }
                
            }

            return lowRows;
        }

        public Mario ParseMario()
        {
            Vector2 marioPos = new Vector2
            {
                X = 16 * int.Parse(level.Element("mario").Element("column").Value),
                Y = 16 * int.Parse(level.Element("mario").Element("row").Value)
            };

            mario = new Mario(marioPos, new Vector2(0, 0), new Vector2(0, 0), graphics, maxCoords);

            return mario;
        }

        private void ParseFireballs(List<IGameObject> list)
        {
            //Add Fireballs to list. Must Be added second & onward in list
            int numOfFireBalls = 2;

            for (int i = 1; i <= numOfFireBalls; i++)           // add fireballs to object list
            {
                FireBall fireball = new FireBall(true, mario);
                list.Add(fireball);
            }

            int listIndex = list.Count;

            for (int i = listIndex - 2; i < listIndex - 1; i++)         // Add reference to next fireball to all fireballs in list but last
            {
                ((FireBall)list[i]).setNextFireBall((FireBall)list[i + 1]);
            }
        }

        private static bool ParseBool(String boolString)
        {
            return boolString == "true";
        }

        private void ParseWarpPipes(XElement chunk, List<IGameObject> list)
        {
            if (chunk.Element("warpPipes").HasElements)
            {
                IEnumerable<XElement> pipes = chunk.Element("warpPipes").Elements();
                bool canWarp = false;
                IGameObject hiddenObj = null;
                WarpPipe pipeToAdd;
                int xPos = 0, yPos = 0;

                foreach (XElement pipe in pipes)
                {
                    if (pipe.HasAttributes)
                    {
                        XAttribute warpAttribute = pipe.Attribute("canWarp");
                        XAttribute xPosAttribute = pipe.Attribute("xPos");
                        XAttribute yPosAttribute = pipe.Attribute("yPos");
                        XAttribute objAttribute = pipe.Attribute("obj");

                        if (warpAttribute != null)
                        {
                            canWarp = ParseBool(warpAttribute.Value);
                        }

                        if (xPosAttribute != null)
                        {
                            xPos = 16 * int.Parse(xPosAttribute.Value);
                        }

                        if (yPosAttribute != null)
                        {
                            yPos = 16 * int.Parse(yPosAttribute.Value);
                        }
                        if (objAttribute != null)
                        {

                            Vector2 objPos = new Vector2
                            {
                                Y = 16 * int.Parse(pipe.Element("row").Value),
                                X = 16 * int.Parse(pipe.Element("column").Value) + baseHeight
                            };
                            System.Diagnostics.Debug.WriteLine("Object: " + objAttribute.Value + " In pipe!");

                            switch (objAttribute.Value)
                            {
                                case "goomba":
                                    hiddenObj = new Goomba(objPos, new Vector2(0, 0), new Vector2(0, 0), list, camera);
                                    break;
                                case "piranha":
                                    objPos.Y -= 8;
                                    hiddenObj = new Piranha(objPos, new Vector2(0, 0), new Vector2(0, 0));
                                    break;
                                case "mushroom":
                                    hiddenObj = new SuperMushroom(new Vector2(objPos.X, objPos.Y), itemSprites, mario);
                                    break;
                                case "fire":
                                    hiddenObj = new FireFlower(new Vector2(objPos.X, objPos.Y), itemSprites, mario);
                                    break;
                                case "star":
                                    hiddenObj = new Star(new Vector2(objPos.X, objPos.Y), itemSprites, mario);
                                    break;
                                case "oneUp":
                                    hiddenObj = new OneUpMushroom(new Vector2(objPos.X, objPos.Y), itemSprites, mario);
                                    break;
                                case "coin":
                                    hiddenObj = new Coin(new Vector2(objPos.X, objPos.Y), itemSprites, mario);
                                    break;
                                default:
                                    //default to goomba on invalid type
                                    hiddenObj = new Goomba(objPos, new Vector2(0, 0), new Vector2(0, 0), list, camera);
                                    break;
                            }

                            list.Add(hiddenObj);
                        }
                    }

                    //Still need to add coins to block
                    Vector2 pipePos = new Vector2
                    {
                        Y = 16 * int.Parse(pipe.Element("row").Value) + baseHeight,
                        X = 16 * int.Parse(pipe.Element("column").Value)
                    };

                    if (!canWarp) pipeToAdd = new WarpPipe(pipePos, new Vector2(0, 0), new Vector2(0, 0), camera);
                    else pipeToAdd = new WarpPipe(pipePos, new Vector2(0, 0), new Vector2(0, 0), true, new Vector2(xPos, yPos), camera);

                    if (hiddenObj != null)
                    {
                        hiddenObj.SetIsPiped(true);
                        pipeToAdd.setPipedObject(hiddenObj);
                    }

                    pipeToAdd.Sprite = new Sprite(false, true, pipePos, pipeSprite, 1, 1, 0, 0);
                    list.Add(pipeToAdd);
                }
            }
            
        }

        private void ParseFloorBlocks(XElement chunk, List<IGameObject> list)
        {
            if (chunk.Element("floorBlocks").HasElements)
            {
                IEnumerable<XElement> floorRows = chunk.Element("floorBlocks").Element("rows").Elements();

                //Handle each individual row
                foreach (XElement floor in floorRows)
                {

                    string[] columnNumbers = floor.Value.Split(',');
                    //Handle each column in the row
                    if (columnNumbers.Length > 1)
                    {
                        for (int i = 0; i < columnNumbers.Length; i++)
                        {
                            string column = columnNumbers[i];
                            Vector2 floorBlockPos = new Vector2
                            {
                                X = 16 * int.Parse(column),
                                Y = 16 * int.Parse(floor.Attribute("num").Value) + baseHeight
                            };

                            Block tempFloor = new Block(floorBlockPos, blockSprites, mario);
                            tempFloor.SetBlockState(new FloorBlockState(tempFloor));
                            list.Add(tempFloor);

                        }
                    }
                }
            }
        }

        private void ParseEnemies(XElement chunk, List<IGameObject> list)
        {
            IEnumerable<XElement> enemies = chunk.Element("enemies").Elements();

            foreach (XElement enemy in enemies)
            {
                string enemyType = enemy.Attribute("type").Value;
                Vector2 enemyPos = new Vector2
                {
                    X = 16 * int.Parse(enemy.Element("column").Value),
                    Y = 16 * int.Parse(enemy.Element("row").Value) + baseHeight
                };
                IEnemy tempEnemy;
                switch (enemyType)
                {
                    case "goomba":

                        tempEnemy = new Goomba(enemyPos, new Vector2(0, 0), new Vector2(0, 0), list, camera);
                        break;
                    case "koopa":
                        enemyPos.Y -= 8;
                        tempEnemy = new KoopaTroopa(enemyPos, camera);

                        break;
                    case "redKoopa":
                        enemyPos.Y -= 8;
                        tempEnemy = new RedKoopaTroopa(enemyPos, camera);

                        break;

                    default:
                        //default to goomba on invalid type
                        tempEnemy = new Goomba(enemyPos, new Vector2(0, 0), new Vector2(0, 0), list, camera);
                        break;

                }

                list.Add(tempEnemy);
            }
        }

        private void ParseItems(XElement chunk, List<IGameObject> list)
        {
            IEnumerable<XElement> items = chunk.Element("items").Elements();

            foreach (XElement item in items)
            {
                //Still need to add coins to block
                Vector2 itemPos = new Vector2
                {
                    Y = 16 * int.Parse(item.Element("row").Value) + baseHeight,
                    X = 16 * int.Parse(item.Element("column").Value)
                };

                IItem generatedItem = GetItemOfType(item.Attribute("type").Value, itemPos, itemSprites, mario);
                if (generatedItem is Coin) generatedItem = new Coin(itemPos, itemSprites, mario, true);
                list.Add(generatedItem);
            }
        }

        private IEnumerable<XElement> ParseStairBlocks(XElement chunk, List<IGameObject> list)
        {
            IEnumerable<XElement> stairBlocks = chunk.Element("stairBlocks").Elements();

            foreach (XElement stair in stairBlocks)
            {
                //Still need to add coins to block
                Vector2 stairBlockPos = new Vector2
                {
                    Y = 16 * int.Parse(stair.Element("row").Value) + baseHeight,
                    X = 16 * int.Parse(stair.Element("column").Value)
                };
                Block tempStair = new Block(stairBlockPos, blockSprites, mario);
                tempStair.SetBlockState(new StairBlockState(tempStair));
                list.Add(tempStair);
            }

            return stairBlocks;
        }

        private void ParseHiddenBlocks(XElement chunk, List<IGameObject> list)
        {
            IEnumerable<XElement> hiddenBlocks = chunk.Element("hiddenBlocks").Elements();

            foreach (XElement hidden in hiddenBlocks)
            {
                //Still need to add coins to block
                Vector2 hiddenBlockPos = new Vector2
                {
                    Y = 16 * int.Parse(hidden.Element("row").Value) + baseHeight,
                    X = 16 * int.Parse(hidden.Element("column").Value)
                };

                List<IItem> items = new List<IItem>();

                if (hidden.HasAttributes)
                {
                    XAttribute itemAttribute = hidden.Attribute("item");
                    if (itemAttribute != null)
                    {
                        items.Add(GetItemOfType(itemAttribute.Value, hiddenBlockPos, itemSprites, mario));

                    }


                    XAttribute coinCountAttribute = hidden.Attribute("coinCount");
                    int numcoins;
                    if (coinCountAttribute != null && int.TryParse(coinCountAttribute.Value, out numcoins))
                    {
                        for (int i = 0; i < numcoins; i++)
                        {

                            items.Add(GetItemOfType("coin", hiddenBlockPos, itemSprites, mario));
                        }
                    }

                }

                Block tempHidden = new Block(hiddenBlockPos, blockSprites, mario, items);
                tempHidden.SetBlockState(new HiddenBlockState(tempHidden));
                list.Add(tempHidden);
                list.AddRange(items);
            }
        }

        private IEnumerable<XElement> ParseQuestionBlocks(XElement chunk, List<IGameObject> list)
        {
            IEnumerable<XElement> questionBlocks = chunk.Element("questionBlocks").Elements();

            foreach (XElement question in questionBlocks)
            {
                Vector2 questionBlockPos = new Vector2
                {
                    Y = 16 * int.Parse(question.Element("row").Value) + baseHeight,
                    X = 16 * int.Parse(question.Element("column").Value)
                };

                List<IItem> items = new List<IItem>();

                if (question.HasAttributes)
                {
                    items.Add(GetItemOfType(question.Attribute("item").Value, questionBlockPos, itemSprites, mario));
                    list.AddRange(items);
                }

                Block tempQuestion = new Block(questionBlockPos, blockSprites, mario, items);
                tempQuestion.SetBlockState(new QuestionBlockState(tempQuestion));
                list.Add(tempQuestion);
            }

            return questionBlocks;
        }

        private static IItem GetItemOfType(string itemType, Vector2 blockPos, Texture2D itemSprites, Mario mario)
        {
            IItem item;
            switch (itemType)
            {
                case "mushroom":
                    item = new SuperMushroom(new Vector2(blockPos.X, blockPos.Y), itemSprites, mario);
                    break;
                case "fire":
                    item = new FireFlower(new Vector2(blockPos.X, blockPos.Y), itemSprites, mario);
                    break;
                case "star":
                    item = new Star(new Vector2(blockPos.X, blockPos.Y), itemSprites, mario);
                    break;
                case "oneUp":
                    item = new OneUpMushroom(new Vector2(blockPos.X, blockPos.Y), itemSprites, mario);
                    break;
                case "coin":
                    item = new Coin(new Vector2(blockPos.X, blockPos.Y), itemSprites, mario);
                    break;
                default:
                    item = null;
                    break;
            }

            return item;
        }

        private IEnumerable<XElement> ParseBrickBlocks(XElement chunk, List<IGameObject> list)
        {
            IEnumerable<XElement> brickBlocks = chunk.Element("brickBlocks").Elements();

            foreach (XElement brick in brickBlocks)
            {
                Vector2 brickBlockPos = new Vector2
                {
                    Y = 16 * int.Parse(brick.Element("row").Value) + baseHeight,
                    X = 16 * int.Parse(brick.Element("column").Value)
                };

                List<IItem> items = new List<IItem>();

                if (brick.HasAttributes)
                {
                    XAttribute itemAttribute = brick.Attribute("item");
                    if (itemAttribute != null)
                    {
                        items.Add(GetItemOfType(itemAttribute.Value, brickBlockPos, itemSprites, mario));

                    }

                    XAttribute coinCountAttribute = brick.Attribute("coinCount");
                    int numcoins;
                    if (coinCountAttribute != null && int.TryParse(coinCountAttribute.Value, out numcoins))
                    {
                        for (int i = 0; i < numcoins; i++)
                        {
                            items.Add(GetItemOfType("coin", brickBlockPos, itemSprites, mario));
                        }
                    }
                }

                Block tempBrick = new Block(brickBlockPos, blockSprites, mario, items);
                tempBrick.SetBlockState(new BrickBlockState(tempBrick));

                list.AddRange(items);
                list.Add(tempBrick);
            }

            return brickBlocks;
        }
    }
}
