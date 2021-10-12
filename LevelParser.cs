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

namespace LevelParser
{
    class LevelParser
    {
        public static List<IGameObject> ParseLevel(string levelPath, GraphicsDeviceManager g, Texture2D blockSprites, Point maxCoords)
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
                Debug.WriteLine("IO ERROR: Failed to load from file " + levelPath);
                Debug.WriteLine(e.Message);
                return list;
            }
            //Mario MUST be parsed first, so that he is the first object in the list.
            Mario mario = ParseMario(g, list, level, maxCoords);

            //Parse brick blocks
            ParseBrickBlocks(blockSprites, list, level, mario);

            //Parse question blocks
            ParseQuestionBlocks(blockSprites, list, level, mario);

            //Parse hidden blocks
            ParseHiddenBlocks(blockSprites, list, level, mario);

            //Parse Enemies
            ParseEnemies(list, level);
            return list;
        }

        private static void ParseEnemies(List<IGameObject> list, XElement level)
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

                        tempEnemy = new Goomba(enemyPos, new Vector2(0, 0), new Vector2(0, 0), list);
                        break;
                    case "koopa":
                        enemyPos.Y -= 8;
                        tempEnemy = new KoopaTroopa(enemyPos);
                       
                        break;
                    case "redKoopa":
                        enemyPos.Y -= 8;
                        tempEnemy = new RedKoopaTroopa(enemyPos);
                       
                        break;

                    default:
                        //default to goomba on invalid type
                        tempEnemy = new Goomba(enemyPos, new Vector2(0, 0), new Vector2(0, 0), list);
                        break;

                }

                list.Add(tempEnemy);
            }
        }

        private static void ParseHiddenBlocks(Texture2D blockSprites, List<IGameObject> list, XElement level, Mario mario)
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
                Block tempHidden = new Block(hiddenBlockPos, blockSprites, mario);
                tempHidden.SetBlockState(new HiddenBlockState(tempHidden));
                list.Add(tempHidden);
            }
        }

        private static void ParseQuestionBlocks(Texture2D blockSprites, List<IGameObject> list, XElement level, Mario mario)
        {
            IEnumerable<XElement> questionBlocks = level.Element("questionBlocks").Elements();

            foreach (XElement question in questionBlocks)
            {


                Vector2 questionBlockPos = new Vector2
                {
                    Y = 16 * Int32.Parse(question.Element("row").Value),
                    X = 16 * Int32.Parse(question.Element("column").Value)
                };
                HashSet<IItem> items = new HashSet<IItem>();
                items.Add(DetermineQuestionItem(question.Attribute("item").Value, questionBlockPos));
                Block tempQuestion = new Block(questionBlockPos, blockSprites, mario,items);
                tempQuestion.SetBlockState(new QuestionBlockState(tempQuestion));
                list.Add(tempQuestion);
            }
        }

        private static IItem DetermineQuestionItem(string itemType, Vector2 blockPos)
        {
            IItem item = new Item(new Vector2(blockPos.X, blockPos.Y));
            IItemState state;
            switch (itemType)
            {
                case "mushroom":
                    state = new SuperMushroomState(item);
                    break;
                case "fire":
                    state = new FireFlowerState(item);
                    break;
                case "star":
                    state = new StarState(item);
                    break;
                case "oneUp":
                    state = new OneUpMushroomState(item);
                    break;
                case "coin":
                    state = new CoinState(item);
                    break;
                default: 
                    state = null; 
                    break;
            }
            item.SetItemState(state);
            return item;
        }

        private static void ParseBrickBlocks(Texture2D blockSprites, List<IGameObject> list, XElement level, Mario mario)
        {
            IEnumerable<XElement> brickRows = level.Element("brickBlocks").Element("rows").Elements();
            int rowNumber = 0;

            //Handle each individual row
            foreach (XElement brick in brickRows)
            {

                string[] columnNumbers = brick.Value.Split(',');
                //Handle each column in the row
                if (columnNumbers.Length > 1)
                {
                    foreach (string column in columnNumbers)
                    {


                       

                        //Separate the row number from the coin count
                        string[] splitRow = column.Split("@");

                        Vector2 brickBlockPos = new Vector2
                        {
                            X = 16 * Int32.Parse(splitRow[0]),
                            Y = 16 * rowNumber
                        };

                        //Handle coins
                        HashSet<IItem> coins = new HashSet<IItem>();
                        if (splitRow.Length > 1)
                        {   
                            int numCoins = Int32.Parse(splitRow[1]);
                            for (int i = 0; i < numCoins; i++)
                            {
                                Vector2 coinPos = new Vector2(brickBlockPos.X, brickBlockPos.Y);
                                IItem coin = new Item(coinPos);
                                coins.Add(coin);
                            }
                            Debug.WriteLine("THIS BLOCK CONTAINS "+numCoins+"COINS: (" + brickBlockPos.X + ", " + brickBlockPos.Y + ")");
                        }
                       
                        Block tempBrick = new Block(brickBlockPos, blockSprites, mario,coins);
                        tempBrick.SetBlockState(new BrickBlockState(tempBrick));
                        list.Add(tempBrick);

                    }
                }

                rowNumber++;
            }
        }

        private static Mario ParseMario(GraphicsDeviceManager g, List<IGameObject> list, XElement level, Point maxCoords)
        {

            Vector2 marioPos = new Vector2
            {
                X = 16 * Int32.Parse(level.Element("mario").Element("column").Value),
                Y = 16 * Int32.Parse(level.Element("mario").Element("row").Value)
            };
            Mario mario = new Mario(marioPos, new Vector2(0, 0), new Vector2(0, 0), g, maxCoords, list);
            list.Add(mario);
            return mario;
        }
    }
}
