// Maxwell Ortwig

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
using View;
using Cameras;
using Microsoft.Xna.Framework.Media;

namespace Game1
{
    public class MarioGame : Game
    {
        private readonly int levelWidth = 3500;
        private readonly int levelHeight = 480;
        private readonly string levelToLoad = "level11";

        private Point maxCoords; 
        private List<IGameObject> objects;
        private List<IGameObject> initialObjects;

        private static int livesRemaining = 3;

        private bool gameIsOver = false;

        //Monogame Objects
        private GraphicsDeviceManager graphics;
        private SpriteFont arial;
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
        private FireBallSpriteFactory fireBallSpriteFactory;
        //For level parser
        private Texture2D blockSprites;
        private Texture2D pipeSprite;
        private Texture2D itemSprites;
        private Texture2D flagSprite;
        private Texture2D castleSprite;
        private string levelPath;

        //Background textures
        private Background background;
        private Camera camera;
        private Vector2 parallax;

        private Song soundtrack;
        private bool isMuted = false;

        private Mario mario;
        
        public MarioGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public static void DecrementLivesRemaining()
        {
            livesRemaining--;
        }

        // Resets objects back to their initial state
        public void ResetObjects()
        {
            objects = initialObjects;
            initialObjects = LevelParser.LevelParser.ParseLevel(levelPath, graphics, blockSprites, maxCoords, pipeSprite, itemSprites, flagSprite, castleSprite, camera);
            mario = (Mario)objects[0];
            InitializeCommands();
            //camera.LookAt(mario.GetPosition());
            background = new Background(GraphicsDevice, spriteBatch, this, mario, camera);
            background.LoadContent();
        }
        public void muteMusic()
        {
            isMuted = !isMuted;
            MediaPlayer.IsMuted = !isMuted;
        }

        protected override void Initialize()
        {;
            keyboardController = new KeyboardController();
            gamepadController = new GamepadController();
            collisionHandler = new CollisionHandler();

            marioSpriteFactory = MarioSpriteFactory.Instance;
            goombaSpriteFactory = GoombaSpriteFactory.Instance;
            koopaTroopaSpriteFactory = KoopaTroopaSpriteFactory.Instance;
            redKoopaTroopaSpriteFactory = RedKoopaTroopaSpriteFactory.Instance;
            fireBallSpriteFactory = FireBallSpriteFactory.Instance;

            camera = new Camera(GraphicsDevice.Viewport);
            camera.Limits = new Rectangle(0, 0, levelWidth, levelHeight);
            maxCoords = new Point(levelWidth, levelHeight);

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
            ICommand throwFireBall = new throwFireballCommand((FireBall)objects[1]);

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
            keyboardController.AddMapping((int)Keys.Space, throwFireBall);

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
            gamepadController.AddMapping((int)Buttons.B, throwFireBall);

            //Music Background Mute
            keyboardController.AddMapping((int)Keys.M, new muteBackgroundMusicCommand(this));

            // Level Reset
            keyboardController.AddMapping((int)Keys.R, new LevelResetCommand(this));

            // AABB Visualization
            keyboardController.AddMapping((int)Keys.C, new BorderVisibleCommand(objects));
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            arial = Content.Load<SpriteFont>("ArialSpriteFont");

            marioSpriteFactory.LoadTextures(this);
            goombaSpriteFactory.LoadTextures(this);
            koopaTroopaSpriteFactory.LoadTextures(this);
            redKoopaTroopaSpriteFactory.LoadTextures(this);
            fireBallSpriteFactory.LoadTextures(this);

            blockSprites = Content.Load<Texture2D>("BlocksV3");
            pipeSprite = Content.Load<Texture2D>("longPipe");
            itemSprites = Content.Load<Texture2D>("Items");
            flagSprite = Content.Load<Texture2D>("Flag");
            castleSprite = Content.Load<Texture2D>("castle");
            itemSpriteFactory = new ItemSpriteFactory(itemSprites);

            // Load from Level file
            
            levelPath = Path.GetFullPath(Content.RootDirectory+ "\\Levels\\" + levelToLoad + ".xml");
            initialObjects = LevelParser.LevelParser.ParseLevel(levelPath, graphics, blockSprites, maxCoords, pipeSprite, itemSprites, flagSprite, castleSprite, camera);
            objects = LevelParser.LevelParser.ParseLevel(levelPath, graphics, blockSprites, maxCoords, pipeSprite, itemSprites, flagSprite, castleSprite, camera);

            mario = (Mario) objects[0];
            InitializeCommands();
            
            background = new Background(GraphicsDevice, spriteBatch, this, mario, camera);
            background.LoadContent();

            //Background Music
            soundtrack = Content.Load<Song>("soundtrack/mainOverworld");
            MediaPlayer.Play(soundtrack);
            MediaPlayer.IsRepeating = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (!gameIsOver)
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

                background.Update();

                objects.RemoveAll(delegate (IGameObject obj)
                {
                    return obj.isQueuedForDeletion();
                });

                if (livesRemaining <= 0) gameIsOver = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!gameIsOver)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                background.Draw();

                parallax = new Vector2(1f);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(parallax));

                // call draw methods from each sprite and pass in sprite batch
                foreach (var obj in objects)
                {
                    obj.Draw(spriteBatch);
                }

                spriteBatch.End();
            } else
            {
                GraphicsDevice.Clear(Color.Black);

                spriteBatch.Begin();
                spriteBatch.DrawString(arial, "Game Over", new Vector2(330, 250), Color.White);
                spriteBatch.End();
            }

            // Draw the legend for player feedback
            spriteBatch.Begin();
            spriteBatch.DrawString(arial, "Mario", new Vector2(20, 20), Color.White);
            spriteBatch.DrawString(arial, "000000", new Vector2(20, 50), Color.White);
            spriteBatch.DrawString(arial, "Coinsx00", new Vector2(230, 50), Color.White);

            if (livesRemaining > 9) spriteBatch.DrawString(arial, "Livesx" + livesRemaining, new Vector2(490, 50), Color.White);
            else spriteBatch.DrawString(arial, "Livesx0" + livesRemaining, new Vector2(490, 50), Color.White);
            spriteBatch.DrawString(arial, "Time", new Vector2(730, 20), Color.White);
            spriteBatch.DrawString(arial, "000", new Vector2(740, 50), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

