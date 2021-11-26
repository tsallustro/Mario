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
        Point maxCoords;
        Camera camera;
        Texture2D blockSprites;
        Texture2D pipeSprite;
        Texture2D itemSprites;

        public ChunkParser(string levelPath, GraphicsDeviceManager graphics, Point maxCoords, Camera camera, Texture2D blockSprites, Texture2D pipeSprite, Texture2D itemSprites)
        {
            try
            {
                level = XElement.Load(levelPath);
            }
            catch (IOException e)
            {
                // Failed to load the level, return an empty list.
                Console.Error.WriteLine("IO ERROR: Failed to load from file " + levelPath);
                Console.Error.WriteLine(e.Message);
            }

            this.graphics = graphics;
            this.maxCoords = maxCoords;
            this.camera = camera;
            this.blockSprites = blockSprites;
            this.pipeSprite = pipeSprite;
            this.itemSprites = itemSprites;
        }

        public static List<IGameObject> ParseLevel(string levelPath, GraphicsDeviceManager g, Texture2D blockSprites, Point maxCoords, Texture2D pipeSprite, Texture2D itemSprites, Texture2D flagSprite, Texture2D castleSprite, Camera camera)
        {
            //Parse Warp Pipes
            /*ParseWarpPipes(list, level, pipeSprite, camera, mario, itemSprites);

            //Parse floor blocks
            ParseFloorBlocks(blockSprites, list, level, mario);

            //Parse brick blocks
            ParseBrickBlocks(blockSprites, list, level, mario, itemSprites);

            //Parse question blocks
            ParseQuestionBlocks(blockSprites, list, level, mario, itemSprites);

            //Parse hidden blocks
            ParseHiddenBlocks(blockSprites, list, level, mario, itemSprites);

            //Parse items
            ParseItems(list, level, itemSprites, mario);

            //Parse Stairs
            ParseStairBlocks(blockSprites, list, level, mario);*/

            //Parse Enemies
            //ParseEnemies(list, level, camera);

            // ParseEnd(list, level, flagSprite, castleSprite);

            return new List<IGameObject>();
        }

        public Chunk ParseChunk(int chunkId)
        {
            return new Chunk();
        }

        private static bool ParseBool(String boolString)
        {
            return boolString == "true";
        }

        private static void ParseWarpPipes(List<IGameObject> list, XElement level, Texture2D pipeSprite, Camera camera, Mario mario, Texture2D itemSprites)
        {
            IEnumerable<XElement> pipes = level.Element("warpPipes").Elements();
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
                        xPos = 16 * Int32.Parse(xPosAttribute.Value);
                    }

                    if (yPosAttribute != null)
                    {
                        yPos = 16 * Int32.Parse(yPosAttribute.Value);
                    }
                    if (objAttribute != null)
                    {

                        Vector2 objPos = new Vector2
                        {
                            Y = 16 * Int32.Parse(pipe.Element("row").Value),
                            X = 16 * Int32.Parse(pipe.Element("column").Value)
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
                    Y = 16 * Int32.Parse(pipe.Element("row").Value),
                    X = 16 * Int32.Parse(pipe.Element("column").Value)
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
        private static void ParseFloorBlocks(Texture2D blockSprites, List<IGameObject> list, XElement level, Mario mario)
        {
            IEnumerable<XElement> floorRows = level.Element("floorBlocks").Element("rows").Elements();


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
                            X = 16 * Int32.Parse(column),
                            Y = 16 * int.Parse(floor.Attribute("num").Value)
                        };

                        Block tempFloor = new Block(floorBlockPos, blockSprites, mario);
                        tempFloor.SetBlockState(new FloorBlockState(tempFloor));
                        list.Add(tempFloor);

                    }
                }


            }
        }
        private static void ParseEnemies(List<IGameObject> list, XElement level, Camera camera)
        {
            IEnumerable<XElement> enemies = level.Element("enemies").Elements();
            foreach (XElement enemy in enemies)
            {
                string enemyType = enemy.Attribute("type").Value;
                Vector2 enemyPos = new Vector2
                {
                    X = 16 * Int32.Parse(enemy.Element("column").Value),
                    Y = 16 * Int32.Parse(enemy.Element("row").Value)
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
        private static void ParseItems(List<IGameObject> list, XElement level, Texture2D itemSprites, Mario mario)
        {
            IEnumerable<XElement> items = level.Element("items").Elements();

            foreach (XElement item in items)
            {
                //Still need to add coins to block
                Vector2 itemPos = new Vector2
                {
                    Y = 16 * Int32.Parse(item.Element("row").Value),
                    X = 16 * Int32.Parse(item.Element("column").Value)
                };

                IItem generatedItem = GetItemOfType(item.Attribute("type").Value, itemPos, itemSprites, mario);
                if (generatedItem is Coin) generatedItem = new Coin(itemPos, itemSprites, mario, true);
                list.Add(generatedItem);
            }
        }

        private static void ParseStairBlocks(Texture2D blockSprites, List<IGameObject> list, XElement level, Mario mario)
        {
            IEnumerable<XElement> stairBlocks = level.Element("stairBlocks").Elements();
            foreach (XElement stair in stairBlocks)
            {
                //Still need to add coins to block
                Vector2 stairBlockPos = new Vector2
                {
                    Y = 16 * Int32.Parse(stair.Element("row").Value),
                    X = 16 * Int32.Parse(stair.Element("column").Value)
                };
                Block tempStair = new Block(stairBlockPos, blockSprites, mario);
                tempStair.SetBlockState(new StairBlockState(tempStair));
                list.Add(tempStair);

            }
        }
        private static void ParseHiddenBlocks(Texture2D blockSprites, List<IGameObject> list, XElement level, Mario mario, Texture2D itemSprites)
        {
            IEnumerable<XElement> hiddenBlocks = level.Element("hiddenBlocks").Elements();
            foreach (XElement hidden in hiddenBlocks)
            {
                //Still need to add coins to block
                Vector2 hiddenBlockPos = new Vector2
                {
                    Y = 16 * Int32.Parse(hidden.Element("row").Value),
                    X = 16 * Int32.Parse(hidden.Element("column").Value)
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

        private static void ParseQuestionBlocks(Texture2D blockSprites, List<IGameObject> list, XElement level, Mario mario, Texture2D itemSprites)
        {
            IEnumerable<XElement> questionBlocks = level.Element("questionBlocks").Elements();

            foreach (XElement question in questionBlocks)
            {
                Vector2 questionBlockPos = new Vector2
                {
                    Y = 16 * Int32.Parse(question.Element("row").Value),
                    X = 16 * Int32.Parse(question.Element("column").Value)
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

        private static void ParseBrickBlocks(Texture2D blockSprites, List<IGameObject> list, XElement level, Mario mario, Texture2D itemSprites)
        {
            IEnumerable<XElement> brickBlocks = level.Element("brickBlocks").Elements();

            foreach (XElement brick in brickBlocks)
            {
                Vector2 brickBlockPos = new Vector2
                {
                    Y = 16 * Int32.Parse(brick.Element("row").Value),
                    X = 16 * Int32.Parse(brick.Element("column").Value)
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
                            System.Diagnostics.Debug.WriteLine("Adding coin " + i);
                            items.Add(GetItemOfType("coin", brickBlockPos, itemSprites, mario));
                        }
                    }
                }

                Block tempBrick = new Block(brickBlockPos, blockSprites, mario, items);
                tempBrick.SetBlockState(new BrickBlockState(tempBrick));

                list.AddRange(items);
                list.Add(tempBrick);
            }
        }

        public Mario ParseMario()
        {
            Vector2 marioPos = new Vector2
            {
                X = 16 * Int32.Parse(level.Element("mario").Element("column").Value),
                Y = 16 * Int32.Parse(level.Element("mario").Element("row").Value)
            };

            return new Mario(marioPos, new Vector2(0, 0), new Vector2(0, 0), graphics, maxCoords);
        }
    }
}
