﻿using Microsoft.Xna.Framework;
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
        Mario mario;

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

        public Chunk ParseChunk(int chunkId)
        {
            List<IGameObject> objects = new List<IGameObject>();
            IEnumerable<XElement> chunks = level.Element("chunks").Elements();

            foreach (XElement chunk in chunks)
            {
                if (chunk.HasAttributes)
                {
                    XAttribute id = chunk.Attribute("id");

                    if (id != null && int.Parse(id.Value) == chunkId)
                    {
                        ParseWarpPipes(chunk, objects);
                        ParseFloorBlocks(chunk, objects);
                        ParseBrickBlocks(chunk, objects);
                        ParseQuestionBlocks(chunk, objects);
                        ParseHiddenBlocks(chunk, objects);
                        ParseItems(chunk, objects);
                        ParseStairBlocks(chunk, objects);
                        ParseEnemies(chunk, objects);
                    }
                }
            }

            return new Chunk(objects);
        }

        public Mario ParseMario()
        {
            Vector2 marioPos = new Vector2
            {
                X = 16 * Int32.Parse(level.Element("mario").Element("column").Value),
                Y = 16 * Int32.Parse(level.Element("mario").Element("row").Value)
            };

            mario = new Mario(marioPos, new Vector2(0, 0), new Vector2(0, 0), graphics, maxCoords);

            return mario;
        }

        private static bool ParseBool(String boolString)
        {
            return boolString == "true";
        }

        private void ParseWarpPipes(XElement chunk, List<IGameObject> list)
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
                            X = 16 * int.Parse(pipe.Element("column").Value)
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
                    Y = 16 * int.Parse(pipe.Element("row").Value),
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

        private void ParseFloorBlocks(XElement chunk, List<IGameObject> list)
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
                            Y = 16 * int.Parse(floor.Attribute("num").Value)
                        };

                        Block tempFloor = new Block(floorBlockPos, blockSprites, mario);
                        tempFloor.SetBlockState(new FloorBlockState(tempFloor));
                        list.Add(tempFloor);

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
                    Y = 16 * int.Parse(enemy.Element("row").Value)
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
                    Y = 16 * int.Parse(item.Element("row").Value),
                    X = 16 * int.Parse(item.Element("column").Value)
                };

                IItem generatedItem = GetItemOfType(item.Attribute("type").Value, itemPos, itemSprites, mario);
                if (generatedItem is Coin) generatedItem = new Coin(itemPos, itemSprites, mario, true);
                list.Add(generatedItem);
            }
        }

        private void ParseStairBlocks(XElement chunk, List<IGameObject> list)
        {
            IEnumerable<XElement> stairBlocks = chunk.Element("stairBlocks").Elements();
            foreach (XElement stair in stairBlocks)
            {
                //Still need to add coins to block
                Vector2 stairBlockPos = new Vector2
                {
                    Y = 16 * int.Parse(stair.Element("row").Value),
                    X = 16 * int.Parse(stair.Element("column").Value)
                };
                Block tempStair = new Block(stairBlockPos, blockSprites, mario);
                tempStair.SetBlockState(new StairBlockState(tempStair));
                list.Add(tempStair);

            }
        }

        private void ParseHiddenBlocks(XElement chunk, List<IGameObject> list)
        {
            IEnumerable<XElement> hiddenBlocks = chunk.Element("hiddenBlocks").Elements();
            foreach (XElement hidden in hiddenBlocks)
            {
                //Still need to add coins to block
                Vector2 hiddenBlockPos = new Vector2
                {
                    Y = 16 * int.Parse(hidden.Element("row").Value),
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

        private void ParseQuestionBlocks(XElement chunk, List<IGameObject> list)
        {
            IEnumerable<XElement> questionBlocks = chunk.Element("questionBlocks").Elements();

            foreach (XElement question in questionBlocks)
            {
                Vector2 questionBlockPos = new Vector2
                {
                    Y = 16 * int.Parse(question.Element("row").Value),
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

        private void ParseBrickBlocks(XElement chunk, List<IGameObject> list)
        {
            IEnumerable<XElement> brickBlocks = chunk.Element("brickBlocks").Elements();

            foreach (XElement brick in brickBlocks)
            {
                Vector2 brickBlockPos = new Vector2
                {
                    Y = 16 * int.Parse(brick.Element("row").Value),
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
    }
}
