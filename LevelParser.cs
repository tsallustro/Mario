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
using View;

namespace LevelParser
{
    class LevelParser
    {
        public static List<IGameObject> ParseLevel(string levelPath, GraphicsDeviceManager g, Texture2D blockSprites, Point maxCoords, Texture2D pipeSprite, Texture2D itemSprites, Texture2D flagSprite, Texture2D castleSprite, Camera camera)
        {
            List<IGameObject> list = new List<IGameObject>();
            XElement level;

            try
            {
                level = XElement.Load(levelPath);
            }
            catch (IOException e)
            {
                //Failed to load the level, return an empty list.
                Console.Error.WriteLine("IO ERROR: Failed to load from file " + levelPath);
                Console.Error.WriteLine(e.Message);
                return list;
            }
            //Mario MUST be parsed first, so that he is the first object in the list.
            Mario mario = ParseMario(g, list, level, maxCoords);

            //Add Fireballs to list. Must Be added second & onward in list
            int numOfFireBalls = 2;
            for (int i = 1; i <= numOfFireBalls; i++)           // add fireballs to object list
            {
                FireBall fireball = new FireBall(true, mario);
                list.Add(fireball);
            }
            for (int i = 1; i <= numOfFireBalls-1; i++)         // Add reference to next fireball to all fireballs in list but last
            {
                ((FireBall)list[i]).setNextFireBall((FireBall)list[i + 1]);
            }

            //Parse Warp Pipes
            ParseWarpPipes(list, level, pipeSprite);

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
            ParseStairBlocks(blockSprites, list, level, mario);

            //Parse Enemies
            ParseEnemies(list, level, camera);

            ParseEnd(list, level, flagSprite, castleSprite);

            return list;
        }

        private static void ParseEnd(List<IGameObject> list, XElement level, Texture2D flagSprite, Texture2D castleSprite)
        {

            XElement flagElement = level.Element("endElements").Element("flag");
            XElement castleElement = level.Element("endElements").Element("castle");

            Vector2 flagPos = new Vector2
            {
                Y = 16 * Int32.Parse(flagElement.Element("row").Value),
                X = 16 * Int32.Parse(flagElement.Element("column").Value)
            };
            Vector2 castlePos = new Vector2
            {
                Y = 16 * Int32.Parse(castleElement.Element("row").Value),
                X = 16 * Int32.Parse(castleElement.Element("column").Value)
            };

            Flag flag = new Flag(flagPos, new Vector2(0, 0), new Vector2(0, 0));
            Castle castle = new Castle(castlePos, new Vector2(0, 0), new Vector2(0, 0));

            flag.Sprite = new Sprite(false, true, flagPos, flagSprite, 1, 1, 0, 0);
            castle.Sprite = new Sprite(false, true, castlePos, castleSprite, 1, 1, 0, 0);

            list.Add(flag);
            list.Add(castle);
        }

        private static void ParseWarpPipes(List<IGameObject> list, XElement level, Texture2D pipeSprite)
        {

            IEnumerable<XElement> pipes = level.Element("warpPipes").Elements();
            foreach (XElement pipe in pipes)
            {
                //Still need to add coins to block
                Vector2 pipePos = new Vector2
                {
                    Y = 16 * Int32.Parse(pipe.Element("row").Value),
                    X = 16 * Int32.Parse(pipe.Element("column").Value)
                };
                WarpPipe pipeToAdd = new WarpPipe(pipePos, new Vector2(0,0), new Vector2(0,0));
                pipeToAdd.Sprite = new Sprite(false, true, pipePos, pipeSprite,1,1,0,0);
                list.Add(pipeToAdd);
                
            }
        }
        private static void ParseFloorBlocks(Texture2D blockSprites, List<IGameObject> list, XElement level, Mario mario)
        {
            IEnumerable<XElement> floorRows = level.Element("floorBlocks").Element("rows").Elements();
            int rowNumber = 0;

            //Handle each individual row
            foreach (XElement floor in floorRows)
            {

                string[] columnNumbers = floor.Value.Split(',');
                //Handle each column in the row
                if (columnNumbers.Length > 1)
                {
                    for (int i = 1; i < columnNumbers.Length; i++)
                    {
                        string column = columnNumbers[i];
                        Vector2 brickBlockPos = new Vector2
                        {
                            X = 16 * Int32.Parse(column),
                            Y = 16 * rowNumber
                        };

                        Block tempFloor = new Block(brickBlockPos, blockSprites, mario);
                        tempFloor.SetBlockState(new FloorBlockState(tempFloor));
                        list.Add(tempFloor);
                        
                    }
                }

                rowNumber++;
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

                //list.Add(generatedItem);
               
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
                    items.Add(GetItemOfType(hidden.Attribute("item").Value, hiddenBlockPos, itemSprites, mario));
                    list.AddRange(items);
                }

                Block tempHidden = new Block(hiddenBlockPos, blockSprites, mario, items);
                tempHidden.SetBlockState(new HiddenBlockState(tempHidden));
                list.Add(tempHidden);
                
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
                    items.Add(GetItemOfType(brick.Attribute("item").Value, brickBlockPos, itemSprites, mario));
                    list.AddRange(items);
                }

                Block tempBrick = new Block(brickBlockPos, blockSprites, mario, items);
                tempBrick.SetBlockState(new BrickBlockState(tempBrick));
                list.Add(tempBrick);

            }
        }

        private static Mario ParseMario(GraphicsDeviceManager g, List<IGameObject> list, XElement level, Point maxCoords)
        {

            Vector2 marioPos = new Vector2
            {
                X = 16 * Int32.Parse(level.Element("mario").Element("column").Value),
                Y = 16 * Int32.Parse(level.Element("mario").Element("row").Value)
            };
            Mario mario = new Mario(marioPos, new Vector2(0, 0), new Vector2(0, 0), g, maxCoords);
            list.Add(mario);
            return mario;
        }
    }
}
