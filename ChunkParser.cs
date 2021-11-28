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

        /*
         *  TODO - At beginning of game, parse all chunks and store them in chunkMap, mapped
         *  via their ID. We can then use this to calculate the compatible chunks for each
         *  chunk, which we store in compatible chunks.
         */
        private Dictionary<int, Chunk> chunkMap;
        private Dictionary<Chunk, List<Chunk>> compatibleChunks;
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
            compatibleChunks = new Dictionary<Chunk, List<Chunk>>();
            numberOfChunks = 0;
        }

        public void ParseAllChunks()
        {
            IEnumerable<XElement> chunks = level.Element("chunks").Elements();

            foreach (XElement chunk in chunks)
            {
                if (chunk.HasAttributes)
                {
                    List<IGameObject> objects = new List<IGameObject>();
                    XAttribute id = chunk.Attribute("id");

                    ParseWarpPipes(chunk, objects);
                    ParseFloorBlocks(chunk, objects);
                    ParseBrickBlocks(chunk, objects);
                    ParseQuestionBlocks(chunk, objects);
                    ParseHiddenBlocks(chunk, objects);
                    ParseItems(chunk, objects);
                    ParseStairBlocks(chunk, objects);
                    ParseEnemies(chunk, objects);
                    ParseFireballs(objects);

                    /*
                     *  Note: Currently, using this method will result in all chunks being at the same
                     *  height. Need to find a way to get them at different heights.
                     */

                    chunkMap.Add(int.Parse(id.Value), new Chunk(objects));
                    numberOfChunks++;
                }
            }
        }

        public void DetermineCompatibleChunks()
        {
            // For each chunk in list (change Dictionary to List) = i
            for (int i = 0; i < numberOfChunks; i++)
            {
                // foreach Chunk that is not the one we are already looking at = j
                    // Calculate height difference of i high and j low, ensure it's within 5 (or whatever max jump is)
                    // foreach value of 1 in highestRowArray of i
                        // Check for >= 2 wide gap in lowestRowArray of chunk j around each value of 1 in i's highestRowArray
                        /// If there is suitable gap, ensure that there is a block that can be reached in j's lowestRowArray
                        /// from the current block in i's highestRowArray
            }
        }

        public Chunk ParseChunk(int chunkId)
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

            if (chunkId != 1)
                return new Chunk(objects);
            else
            {
                System.Diagnostics.Debug.WriteLine(GetHighRows(allBlocks));
                return new Chunk(objects, GetHighRows(allBlocks), GetLowRows(allBlocks));
            }
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
            int[,] lowRows = new int[5, 50];

            foreach (XElement block in blocks)
            {
                int Y = int.Parse(block.Element("row").Value);

                if (Y >= 25 && Y < 30)
                {
                    int X = int.Parse(block.Element("column").Value);
                    Y -= 25; // Ensure Y is in range of array
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
