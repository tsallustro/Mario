// Maxwell Ortwig

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

namespace Game1
{
    public class MarioGame : Game
    {

        private readonly string levelPath = "";
        private Point maxCoords; 
        List<IGameObject> objects;
        
        //Monogame Objects
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Controllers
        private IController keyboardController;
        private IController gamepadController;

        //Game Objects
        //Enemy objects
        private IEnemy goomba;
        private IEnemy koopaTroopa;
        private IEnemy redKoopaTroopa;

        //Character ojbects
        private IAvatar mario;

        //Obstacle objects
        private IBlock questionBlock;
        private IBlock usedBlock; 
        private IBlock brickBlock;
        private IBlock floorBlock;
        private IBlock stairBlock;
        private IBlock hiddenBlock;

        //Item objects
        private IItem item;
        private IItem fireFlower;
        private IItem coin;
        private IItem superMushroom;
        private IItem oneUpMushroom;
        private IItem star;

        //Sprite factories
        private MarioSpriteFactory marioSpriteFactory;
        private GoombaSpriteFactory goombaSpriteFactory;
        private ItemSpriteFactory itemSpriteFactory;
        private KoopaTroopaSpriteFactory koopaTroopaSpriteFactory;
        private RedKoopaTroopaSpriteFactory redKoopaTroopaSpriteFactory;

        public MarioGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            keyboardController = new KeyboardController();
            gamepadController = new GamepadController();

            marioSpriteFactory = MarioSpriteFactory.Instance;
            goombaSpriteFactory = GoombaSpriteFactory.Instance;
            itemSpriteFactory = ItemSpriteFactory.Instance;
            koopaTroopaSpriteFactory = KoopaTroopaSpriteFactory.Instance;
            redKoopaTroopaSpriteFactory = RedKoopaTroopaSpriteFactory.Instance;



            maxCoords = new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            this.Window.Title = "Cornet Mario Game";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
           
            marioSpriteFactory.LoadTextures(this);

            /*
             * Since we will have multiple enemies and items, we should make
             * these factories not be singletons.
             */
            goombaSpriteFactory.LoadTextures(this);
            itemSpriteFactory.LoadTextures(this);
            koopaTroopaSpriteFactory.LoadTextures(this);
            redKoopaTroopaSpriteFactory.LoadTextures(this);
            Texture2D blockSprites = Content.Load<Texture2D>("BlocksV3");

            // Visuals for Sprint 1
            mario = new Mario(new Vector2(10, graphics.PreferredBackBufferHeight - 30), new Vector2(0, 0),
                new Vector2(0,0), graphics, maxCoords);
            goomba = new Goomba(new Vector2(50, graphics.PreferredBackBufferHeight - 80), new Vector2(0,0), new Vector2(0,0), objects);
            koopaTroopa = new KoopaTroopa(new Vector2(350, 100));
            redKoopaTroopa = new RedKoopaTroopa(new Vector2(400, 100));

            questionBlock = new Block(new Vector2(100, 200), blockSprites, (Mario)mario, new HashSet<IItem>{});
            usedBlock = new Block(new Vector2(150, 200), blockSprites, (Mario)mario);
            brickBlock = new Block(new Vector2(200, 200), blockSprites, (Mario)mario, new HashSet<IItem>{});
            floorBlock = new Block(new Vector2(250, 200), blockSprites, (Mario)mario);
            stairBlock = new Block(new Vector2(300, 200), blockSprites, (Mario)mario);
            hiddenBlock = new Block(new Vector2(350, 200), blockSprites, (Mario)mario, new HashSet<IItem>{});

            item = new Item(new Vector2(0, 0));
            coin = new Item(new Vector2(100, 50));
            superMushroom = new Item(new Vector2(150, 50));
            oneUpMushroom = new Item(new Vector2(200, 50));
            fireFlower = new Item(new Vector2(50, 50));
            star = new Item(new Vector2(250, 50));

            /* Add all objects in level to object list! */
            objects.Add(mario);
            objects.Add(goomba);
            objects.Add(koopaTroopa);
            objects.Add(redKoopaTroopa);
            objects.Add(questionBlock);
            objects.Add(usedBlock);
            objects.Add(brickBlock);
            objects.Add(floorBlock);
            objects.Add(stairBlock);
            objects.Add(hiddenBlock);
            objects.Add(coin);
            objects.Add(superMushroom);
            objects.Add(oneUpMushroom);
            objects.Add(fireFlower);
            objects.Add(star);

            // Set obstacle states
            questionBlock.SetBlockState(new QuestionBlockState(questionBlock));
            usedBlock.SetBlockState(new UsedBlockState(usedBlock));
            brickBlock.SetBlockState(new BrickBlockState(brickBlock));
            floorBlock.SetBlockState(new FloorBlockState(floorBlock));
            stairBlock.SetBlockState(new StairBlockState(stairBlock));
            hiddenBlock.SetBlockState(new HiddenBlockState(hiddenBlock));

            // Set item states
            coin.SetItemState(new CoinState(item));
            superMushroom.SetItemState(new SuperMushroomState(item));
            oneUpMushroom.SetItemState(new OneUpMushroomState(item));
            fireFlower.SetItemState(new FireFlowerState(item));
            star.SetItemState(new StarState(item));

            // Initialize commands that will be repeated
            ICommand moveLeft = new MoveLeftCommand(mario);
            ICommand moveRight = new MoveRightCommand(mario);
            ICommand jump = new JumpCommand(mario);
            ICommand crouch = new CrouchCommand(mario);

            // Initialize keyboard controller mappings
            // Action commands
            keyboardController.AddMapping((int)Keys.Q, new QuitCommand(this));
            keyboardController.AddMapping((int)Keys.Left, moveLeft);
            keyboardController.AddMapping((int)Keys.A, moveLeft);
            keyboardController.AddMapping((int)Keys.Right, moveRight);
            keyboardController.AddMapping((int)Keys.D, moveRight);
            keyboardController.AddMapping((int)Keys.Up, jump);
            keyboardController.AddMapping((int)Keys.W, jump);
            keyboardController.AddMapping((int)Keys.Down, crouch);
            keyboardController.AddMapping((int)Keys.S, crouch);

            // Power-up commands
            keyboardController.AddMapping((int)Keys.Y, new StandardMarioCommand(mario));
            keyboardController.AddMapping((int)Keys.U, new SuperMarioCommand(mario));
            keyboardController.AddMapping((int)Keys.I, new FireMarioCommand(mario));
            keyboardController.AddMapping((int)Keys.O, new DeadMarioCommand(mario));

            // Enemy commands
            keyboardController.AddMapping((int)Keys.Z, new IdleGoombaCommand(goomba));
            keyboardController.AddMapping((int)Keys.X, new MovingGoombaCommand(goomba));
            keyboardController.AddMapping((int)Keys.F, new StompedGoombaCommand(goomba));
            keyboardController.AddMapping((int)Keys.V, new IdleKoopaTroopaCommand(koopaTroopa));
            keyboardController.AddMapping((int)Keys.N, new MovingKoopaTroopaCommand(koopaTroopa));
            keyboardController.AddMapping((int)Keys.M, new StompedKoopaTroopaCommand(koopaTroopa));
            keyboardController.AddMapping((int)Keys.J, new IdleRedKoopaTroopaCommand(redKoopaTroopa));
            keyboardController.AddMapping((int)Keys.K, new MovingRedKoopaTroopaCommand(redKoopaTroopa));
            keyboardController.AddMapping((int)Keys.L, new StompedRedKoopaTroopaCommand(redKoopaTroopa));

            // Brick commands
            keyboardController.AddMapping((int)Keys.OemQuestion, new BumpCommand(questionBlock, (Mario) mario));
            keyboardController.AddMapping((int)Keys.B, new BumpCommand(brickBlock, (Mario) mario));
            keyboardController.AddMapping((int)Keys.H, new BumpCommand(hiddenBlock, (Mario) mario));

            // AABB Visualization
            keyboardController.AddMapping((int)Keys.C, new BorderVisibleCommand(objects));

            // Initialize gamepad controller mappings
            gamepadController.AddMapping((int)Buttons.DPadLeft, moveLeft);
            gamepadController.AddMapping((int)Buttons.DPadRight, moveRight);
            gamepadController.AddMapping((int)Buttons.A, jump);
            gamepadController.AddMapping((int)Buttons.DPadDown, crouch);

            //Load from Level file
            objects = ParseLevel(levelPath, graphics, blockSprites);
        }
        protected override void Update(GameTime gameTime)
        {
            // Update the controllers
            gamepadController.Update();
            keyboardController.Update();

            foreach (var obj in objects)
            {
                obj.Update(gameTime);
            }
            
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            // call draw methods from each sprite and pass in sprite batch
            foreach (var obj in objects)
            {
                obj.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        private List<IGameObject> ParseLevel(string levelPath, GraphicsDeviceManager g, Texture2D blockSprites)
        {
            List<IGameObject> list = new List<IGameObject>();
            XElement level = XElement.Load(levelPath);

            //Parse Mario
            Vector2 marioPos = new Vector2();
            marioPos.X = 16 * Int32.Parse(level.Element("mario").Element("row").Value);
            marioPos.Y = 16 * Int32.Parse(level.Element("mario").Element("column").Value);
            Mario mario = new Mario(marioPos, new Vector2(0,0), new Vector2(0,0), g, maxCoords);
            list.Add(mario);

            //Parse brick blocks
            IEnumerable<XElement> brickRows = level.Element("brickBlocks").Element("rows").Descendants();
            int rowNumber = 0;

            //Handle each individual row
            foreach (XElement brick in brickRows)
            {

                string[] columnNumbers = brick.Value.Split(',');
                //Handle each column in the row
                foreach (string column in columnNumbers)
                {
                    Vector2 brickBlockPos = new Vector2();
                    brickBlockPos.X = 16 * Int32.Parse(column);
                    brickBlockPos.Y = 16 * rowNumber;
                    Block tempBrick = new Block(brickBlockPos, blockSprites,mario);
                    tempBrick.SetBlockState(new BrickBlockState(tempBrick));
                    list.Add(tempBrick);
                }

                rowNumber++;
            }

            //Parse question blocks
            IEnumerable<XElement> questionBlocks = level.Element("questionBlocks").Descendants();
            foreach (XElement question in questionBlocks)
            {
                //Still need to add item to block
                Vector2 questionBlockPos = new Vector2();
                questionBlockPos.Y = 16 * Int32.Parse(question.Element("row").Value);
                questionBlockPos.X = 16 * Int32.Parse(question.Element("column").Value);
                Block tempQuestion = new Block(questionBlockPos, blockSprites,mario);
                tempQuestion.SetBlockState(new QuestionBlockState(tempQuestion));
                list.Add(tempQuestion);
            }

            //Parse hidden blocks
            IEnumerable<XElement> hiddenBlocks = level.Element("hiddenBlocks").Descendants();
            foreach (XElement hidden in hiddenBlocks)
            {
                //Still need to add item to block
                Vector2 hiddenBlockPos = new Vector2();
                hiddenBlockPos.Y = 16 * Int32.Parse(hidden.Element("row").Value);
                hiddenBlockPos.X = 16 * Int32.Parse(hidden.Element("column").Value);
                Block tempHidden = new Block(hiddenBlockPos, blockSprites,mario);
                tempHidden.SetBlockState(new HiddenBlockState(tempHidden));
                list.Add(tempHidden);
            }

            //Parse Enemies
            IEnumerable<XElement> enemies = level.Element("enemies").Descendants();
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
                        
                        tempEnemy = new Goomba(enemyPos, new Vector2(0,0), new Vector2(0,0), list);
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
            return list;
        }
    }

   
}

