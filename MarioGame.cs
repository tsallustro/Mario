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
using System.IO;
using LevelParser;


namespace Game1
{
    public class MarioGame : Game
    {


        private readonly string levelToLoad = "dummylevel";
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

            // AABB Visualization
            keyboardController.AddMapping((int)Keys.C, new BorderVisibleCommand(objects));

            // Initialize gamepad controller mappings
            gamepadController.AddMapping((int)Buttons.DPadLeft, moveLeft);
            gamepadController.AddMapping((int)Buttons.DPadRight, moveRight);
            gamepadController.AddMapping((int)Buttons.A, jump);
            gamepadController.AddMapping((int)Buttons.DPadDown, crouch);

            //Load from Level file
            string levelPath = Path.GetFullPath(@"..\..\..\Levels\"+levelToLoad+".xml");
            objects = LevelParser.LevelParser.ParseLevel(levelPath, graphics, blockSprites, maxCoords);
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
        
    }

   
}

