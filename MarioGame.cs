﻿// Maxwell Ortwig

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using Controllers;
using Commands;
using GameObjects;
using Factories;
using States;
using System;
using Collisions;
using LevelParser;


namespace Game1
{
    public class MarioGame : Game
    {
        private readonly string levelToLoad = "sprint2Map";
        private Point maxCoords; 
        private List<IGameObject> objects;
        private List<IGameObject> initialObjects;

        //Monogame Objects
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Controllers
        private IController keyboardController;
        private IController gamepadController;

        //CollisionHandler
        private CollisionHandler collisionHandler;

        //Sprite factories
        private MarioSpriteFactory marioSpriteFactory;
        private GoombaSpriteFactory goombaSpriteFactory;
        private ItemSpriteFactory itemSpriteFactory;
        private KoopaTroopaSpriteFactory koopaTroopaSpriteFactory;
        private RedKoopaTroopaSpriteFactory redKoopaTroopaSpriteFactory;

        //For level parser
        private Texture2D blockSprites;
        private Texture2D pipeSprite;
        private string levelPath;

        private Mario mario;

        public MarioGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // Resets objects back to their initial state
        public void ResetObjects()
        {
            objects = initialObjects;
            initialObjects = LevelParser.LevelParser.ParseLevel(levelPath, graphics, blockSprites, maxCoords, pipeSprite);
            mario = (Mario)objects[0];
            InitializeCommands();
        }

        protected override void Initialize()
        {
            keyboardController = new KeyboardController();
            gamepadController = new GamepadController();
            collisionHandler = new CollisionHandler();

            marioSpriteFactory = MarioSpriteFactory.Instance;
            goombaSpriteFactory = GoombaSpriteFactory.Instance;
            itemSpriteFactory = ItemSpriteFactory.Instance;
            koopaTroopaSpriteFactory = KoopaTroopaSpriteFactory.Instance;
            redKoopaTroopaSpriteFactory = RedKoopaTroopaSpriteFactory.Instance;

            maxCoords = new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            this.Window.Title = "Cornet Mario Game";
            base.Initialize();
        }

        private void InitializeCommands()
        {
            // Initialize commands that will be repeated
            ICommand moveLeft = new MoveLeftCommand(mario);
            ICommand moveRight = new MoveRightCommand(mario);
            ICommand jump = new JumpCommand(mario);
            ICommand crouch = new CrouchCommand(mario);

            // Initialize keyboard controller mappinqgs
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

            // Initialize gamepad controller mappings
            gamepadController.AddMapping((int)Buttons.DPadLeft, moveLeft);
            gamepadController.AddMapping((int)Buttons.DPadRight, moveRight);
            gamepadController.AddMapping((int)Buttons.A, jump);
            gamepadController.AddMapping((int)Buttons.DPadDown, crouch);

            // Level Reset
            keyboardController.AddMapping((int)Keys.R, new LevelResetCommand(this));

            // AABB Visualization
            keyboardController.AddMapping((int)Keys.C, new BorderVisibleCommand(objects));
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            marioSpriteFactory.LoadTextures(this);
            goombaSpriteFactory.LoadTextures(this);
            itemSpriteFactory.LoadTextures(this);
            koopaTroopaSpriteFactory.LoadTextures(this);
            redKoopaTroopaSpriteFactory.LoadTextures(this);
            blockSprites = Content.Load<Texture2D>("BlocksV3");
            pipeSprite = Content.Load<Texture2D>("pipe");

            // Load from Level file
            levelPath = Path.GetFullPath(@"..\..\..\Levels\" + levelToLoad + ".xml");
            objects = LevelParser.LevelParser.ParseLevel(levelPath, graphics, blockSprites, maxCoords, pipeSprite);
            initialObjects = LevelParser.LevelParser.ParseLevel(levelPath, graphics, blockSprites, maxCoords, pipeSprite);

            mario = (Mario) objects[0];
            InitializeCommands();
        }

        protected override void Update(GameTime gameTime)
        {
            // Update the controllers
            gamepadController.Update();
            keyboardController.Update();

            //Make sure to put update collisiondetection before object update
            collisionHandler.Update(gameTime, objects);
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

