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
                Vector2 enemyPos = new Vector2();
                enemyPos.X = 16 * Int32.Parse(enemy.Element("column").Value);
                enemyPos.Y = 16 * Int32.Parse(enemy.Element("row").Value);
                IEnemy tempEnemy;
                switch (enemyType)
                {
                    case "goomba":

                        tempEnemy = new Goomba(enemyPos, new Vector2(0, 0), new Vector2(0, 0), list);
                        break;
                    case "koopa":

                        tempEnemy = new KoopaTroopa(enemyPos);
                        break;
                    case "redKoopa":

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
                Vector2 hiddenBlockPos = new Vector2();
                hiddenBlockPos.Y = 16 * Int32.Parse(hidden.Element("row").Value);
                hiddenBlockPos.X = 16 * Int32.Parse(hidden.Element("column").Value);
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

                HashSet<Item> items = new HashSet<Item>();
                //items.Add(DetermineQuestionItem(question.Attribute("item").Value));
                Vector2 questionBlockPos = new Vector2();
                questionBlockPos.Y = 16 * Int32.Parse(question.Element("row").Value);
                questionBlockPos.X = 16 * Int32.Parse(question.Element("column").Value);
                Block tempQuestion = new Block(questionBlockPos, blockSprites, mario);
                tempQuestion.SetBlockState(new QuestionBlockState(tempQuestion));
                list.Add(tempQuestion);

            }
        }

        private static Item DetermineQuestionItem(string itemType)
        {
            //NOT IMPLEMENTED YET
            return null;
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

                        //Still need to add coins to block
                        Vector2 brickBlockPos = new Vector2();
                        brickBlockPos.X = 16 * Int32.Parse(column);
                        brickBlockPos.Y = 16 * rowNumber;
                        Block tempBrick = new Block(brickBlockPos, blockSprites, mario);
                        tempBrick.SetBlockState(new BrickBlockState(tempBrick));
                        list.Add(tempBrick);

                    }
                }

                rowNumber++;
            }
        }

        private static Mario ParseMario(GraphicsDeviceManager g, List<IGameObject> list, XElement level, Point maxCoords)
        {

            Vector2 marioPos = new Vector2();
            marioPos.X = 16 * Int32.Parse(level.Element("mario").Element("column").Value);
            marioPos.Y = 16 * Int32.Parse(level.Element("mario").Element("row").Value);
            Mario mario = new Mario(marioPos, new Vector2(0, 0), new Vector2(0, 0), g, maxCoords, list);
            list.Add(mario);
            return mario;
        }
    }
}
