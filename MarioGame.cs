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
using Sprites;
using Collisions;
using LevelParser;
using View;
using Cameras;
using Microsoft.Xna.Framework.Media;
using Sound;
using Microsoft.Xna.Framework.Audio;

namespace Game1
{
    public class MarioGame : Game
    {
        private readonly int levelWidth = 3500;
        private readonly int levelHeight = 480;
        private readonly string levelToLoad = "level11";
        private readonly double timeLimit = 400;
        private double secondsRemaining = 400;
        private bool playedWarningSound = false;
        private Point maxCoords; 
        private List<IGameObject> objects;
        private List<IGameObject> initialObjects;
        private List<ICommand> commands;

        private bool gameIsOver = false;
        private bool gameIsWon = false;
        private bool paused = false;

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

        //Sprites for UI
        private ISprite coinsIcon;
        private ISprite livesIcon;

        //Background textures
        private Background background;
        private Camera camera;
        private Vector2 parallax;

        //Checkpoints
        private Vector2 checkPoint = new Vector2(0,400);
        int passed = 0;

        private Mario mario;
        private int coinsCollected = 0;
        public MarioGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // Resets objects back to their initial state
        public void ResetObjects()
        {
            SoundManager.Instance.Reset();
            SoundManager.Instance.StartMusic();

            gameIsOver = false;
            gameIsWon = false;
            secondsRemaining = timeLimit;
            coinsCollected = 0;
            playedWarningSound = false;
            objects = initialObjects;
            initialObjects = LevelParser.LevelParser.ParseLevel(levelPath, graphics, blockSprites, maxCoords, pipeSprite, itemSprites, flagSprite, castleSprite, camera);
            
            mario = (Mario)objects[0];

            InitializeCommands();
            checkPoint = mario.GetPosition();
            background = new Background(GraphicsDevice, spriteBatch, this, mario, camera);
            background.LoadContent();
        }
        public void ResetCheckPoint(Vector2 position)
        {
            ResetObjects();
            mario.SetPosition(position);
        }


        public void TogglePause()
        {
            paused = !paused;

            SoundManager.Instance.TogglePaused();
        }

        private void SetCommandsForPause()
        {
            foreach (ICommand command in commands)
            {
                if (command is PauseGameCommand || command is QuitCommand) command.SetActive(true);
                else command.SetActive(false);
            } 
        }

        private void SetCommandsForWinningState()
        {
            foreach (ICommand command in commands)
            {
                if (command is MarioCommand || command is MuteCommand || command is PauseGameCommand || command is throwFireballCommand) 
                    command.SetActive(false);
                else command.SetActive(true);
            }
        }

        private void SetCommandsForGameOver()
        {
            foreach (ICommand command in commands)
            {
                if (command is LevelResetCommand || command is QuitCommand) command.SetActive(true);
                else command.SetActive(false);
            }
        }

        private void EnableAllCommands()
        {
            foreach (ICommand command in commands) command.SetActive(true);
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
            commands = new List<ICommand>();

            // Initialize commands that will be repeated
            ICommand moveLeft = new MoveLeftCommand(mario);
            ICommand moveRight = new MoveRightCommand(mario);
            ICommand jump = new JumpCommand(mario);
            ICommand crouch = new CrouchCommand(mario);
            ICommand throwFireBall = new throwFireballCommand((FireBall)objects[1]);
            ICommand quit = new QuitCommand(this);
            ICommand standard = new StandardMarioCommand(mario);
            ICommand super = new SuperMarioCommand(mario);
            ICommand fire = new FireMarioCommand(mario);
            ICommand dead = new DeadMarioCommand(mario);
            ICommand mute = new MuteCommand(this);
            ICommand reset = new LevelResetCommand(this);
            ICommand borderVis = new BorderVisibleCommand(objects);
            ICommand pause = new PauseGameCommand(this);

            commands.Add(moveLeft);
            commands.Add(moveRight);
            commands.Add(jump);
            commands.Add(crouch);
            commands.Add(throwFireBall);
            commands.Add(quit);
            commands.Add(standard);
            commands.Add(super);
            commands.Add(fire);
            commands.Add(dead);
            commands.Add(mute);
            commands.Add(reset);
            commands.Add(borderVis);
            commands.Add(pause);

            // Initialize keyboard controller mappinqgs
            // Action commands
            keyboardController.AddMapping((int)Keys.Q, quit);
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
            keyboardController.AddMapping((int)Keys.Y, standard);
            keyboardController.AddMapping((int)Keys.U, super);
            keyboardController.AddMapping((int)Keys.I, fire);
            keyboardController.AddMapping((int)Keys.O, dead);

            // Initialize gamepad controller mappings
            gamepadController.AddMapping((int)Buttons.DPadLeft, moveLeft);
            gamepadController.AddMapping((int)Buttons.DPadRight, moveRight);
            gamepadController.AddMapping((int)Buttons.A, jump);
            gamepadController.AddMapping((int)Buttons.DPadDown, crouch);
            gamepadController.AddMapping((int)Buttons.B, throwFireBall);

            //Music Background Mute
            keyboardController.AddMapping((int)Keys.M, mute);

            // Level Reset
            keyboardController.AddMapping((int)Keys.R, reset);

            // AABB Visualization
            keyboardController.AddMapping((int)Keys.C, borderVis);

            // Pause
            keyboardController.AddMapping((int)Keys.P, pause);
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

            coinsIcon = new Sprite(false, true, new Vector2(225, 54), Content.Load<Texture2D>("Items"), 1, 9, 7, 8);
            livesIcon = new Sprite(false, true, new Vector2(474, 54), Content.Load<Texture2D>("standardMario"), 1, 15, 0, 0);

            blockSprites = Content.Load<Texture2D>("BlocksV3");
            pipeSprite = Content.Load<Texture2D>("longPipe");
            itemSprites = Content.Load<Texture2D>("Items");
            flagSprite = Content.Load<Texture2D>("Flag");
            castleSprite = Content.Load<Texture2D>("castle");
            itemSpriteFactory = new ItemSpriteFactory(itemSprites);

            //Map Sound effects
            SoundManager.Instance.MapSound(SoundManager.GameSound.STANDARD_JUMP,Content.Load<SoundEffect>("sounds/standard_jump") );
            SoundManager.Instance.MapSound(SoundManager.GameSound.SUPER_JUMP, Content.Load<SoundEffect>("sounds/super_jump"));
            SoundManager.Instance.MapSound(SoundManager.GameSound.DEATH, Content.Load<SoundEffect>("sounds/death"));
            SoundManager.Instance.MapSound(SoundManager.GameSound.COIN, Content.Load<SoundEffect>("sounds/coin"));
            SoundManager.Instance.MapSound(SoundManager.GameSound.POWER_UP_APPEAR, Content.Load<SoundEffect>("sounds/power_up_appears"));
            SoundManager.Instance.MapSound(SoundManager.GameSound.POWER_UP_COLLECTED, Content.Load<SoundEffect>("sounds/power_up"));
            SoundManager.Instance.MapSound(SoundManager.GameSound.ONE_UP_COLLECTED, Content.Load<SoundEffect>("sounds/one_up"));
            SoundManager.Instance.MapSound(SoundManager.GameSound.BUMP, Content.Load<SoundEffect>("sounds/bump"));
            SoundManager.Instance.MapSound(SoundManager.GameSound.BRICK_BREAK, Content.Load<SoundEffect>("sounds/brick_break"));
            SoundManager.Instance.MapSound(SoundManager.GameSound.PIPE_TRAVEL, Content.Load<SoundEffect>("sounds/pipe"));
 
            SoundManager.Instance.MapSound(SoundManager.GameSound.GAME_OVER, Content.Load<SoundEffect>("sounds/game_over"));
            SoundManager.Instance.MapSound(SoundManager.GameSound.STOMP, Content.Load<SoundEffect>("sounds/loud_stomp"));
            SoundManager.Instance.SetBackgroundMusic(Content.Load<Song>("soundtrack/mainOverworld"), Content.Load<Song>("soundtrack/overworld_fast"));
            SoundManager.Instance.StartMusic();

            // Load from Level file

            levelPath = Path.GetFullPath(Content.RootDirectory+ "\\Levels\\" + levelToLoad + ".xml");
            initialObjects = LevelParser.LevelParser.ParseLevel(levelPath, graphics, blockSprites, maxCoords, pipeSprite, itemSprites, flagSprite, castleSprite, camera);
            objects = LevelParser.LevelParser.ParseLevel(levelPath, graphics, blockSprites, maxCoords, pipeSprite, itemSprites, flagSprite, castleSprite, camera);

            mario = (Mario) objects[0];
            InitializeCommands();
            
            background = new Background(GraphicsDevice, spriteBatch, this, mario, camera);
            background.LoadContent();

            //Background Music
            
            
        }

        protected override void Update(GameTime gameTime)
        {
            // Update the controllers
            gamepadController.Update();
            keyboardController.Update();
            
            if (!gameIsOver)
            {
                if (!paused)
                {
                    if (!mario.WinningStateReached) EnableAllCommands();
                    else SetCommandsForWinningState();

                    if (mario.hasWarped)
                    {
                        camera.Limits = new Rectangle(levelWidth + 230, 0, GraphicsDevice.Viewport.Width, levelHeight);
                        maxCoords = new Point(levelWidth + 3000, levelHeight);
                    } else
                    {
                        camera.Limits = new Rectangle(0, 0, levelWidth, levelHeight);
                        maxCoords = new Point(levelWidth, levelHeight);
                    }

                    // Update time remaining
                    if (secondsRemaining <= 0 && !gameIsWon)
                    {
                        mario.Die(); // Time limit reached!
                    } else
                    {
                        secondsRemaining -= gameTime.ElapsedGameTime.TotalSeconds;
                        if(!playedWarningSound && secondsRemaining <= 100)
                        {
                            SoundManager.Instance.TimeWarning();
                            playedWarningSound = true;
                        }
                    }

                    // Make sure to put update collisiondetection before object update
                    collisionHandler.Update(gameTime, objects);

                    foreach (var obj in objects)
                    {
                        obj.Update(gameTime);
                    }

                    background.Update();

                    //Removed all items that are queued for deletion
                    objects.RemoveAll(delegate (IGameObject obj)
                    {
                        if (obj.isQueuedForDeletion() && obj is Coin) CoinCollected();
                        return obj.isQueuedForDeletion();
                    });

                    if (mario.GetLivesRemaining() <= 0) gameIsOver = true;
                    if (mario.WinningStateReached && mario.GetVelocity().Y == 0) gameIsWon = true;
                }
                else
                {
                    SetCommandsForPause();
                }
            } else if (gameIsOver || gameIsWon)
            {
                SetCommandsForGameOver();
            }

            setCheckPoint();

            if (mario.GetPowerState() is DeadMario && mario.GetLivesRemaining() > 0)
            {
                ResetCheckPoint(checkPoint);
            }

            coinsIcon.Update();
            base.Update(gameTime);
        }
        public void setCheckPoint()
        {
            if(mario.GetPosition().X > 1200 && passed == 0)
            {
                checkPoint = new Vector2(1200, 400);
                passed = 1;
            } else if (mario.GetPosition().X >2400 && passed == 1)
            {
                checkPoint = new Vector2(2400, 400);
                passed = 2;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!gameIsOver)
            {
                if (mario.hasWarped) GraphicsDevice.Clear(Color.Black);
                else
                {
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    background.Draw();
                }

                parallax = new Vector2(1f);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(parallax));

                // call draw methods from each sprite and pass in sprite batch
                foreach (var obj in objects)
                {
                    obj.Draw(spriteBatch);
                }

                spriteBatch.End();

                if (gameIsWon)
                {
                    spriteBatch.Begin();
                    spriteBatch.DrawString(arial, "Winner!", new Vector2(338, 190), Color.White);
                    spriteBatch.DrawString(arial, "Replay [R]", new Vector2(200, 275), Color.White);
                    spriteBatch.DrawString(arial, "Quit [Q]", new Vector2(470, 275), Color.White);
                    spriteBatch.End();
                }
            } else
            {
                GraphicsDevice.Clear(Color.Black);

                spriteBatch.Begin();
                spriteBatch.DrawString(arial, "Game Over", new Vector2(330, 250), Color.White);
                spriteBatch.DrawString(arial, "Replay [R]", new Vector2(200, 300), Color.White);
                spriteBatch.DrawString(arial, "Quit [Q]", new Vector2(470, 300), Color.White);
                spriteBatch.End();
            }

            // Draw the legend for player feedback
            spriteBatch.Begin();
            spriteBatch.DrawString(arial, "Mario", new Vector2(20, 20), Color.White);
            spriteBatch.DrawString(arial, "000000", new Vector2(20, 50), Color.White);

            coinsIcon.Draw(spriteBatch, false);
            spriteBatch.DrawString(arial, "x"+coinsCollected, new Vector2(240, 50), Color.White);

            int livesRemaining = mario.GetLivesRemaining();

            livesIcon.Draw(spriteBatch, false);
            spriteBatch.DrawString(arial, "x" + livesRemaining.ToString("D2"), new Vector2(490, 50), Color.White);
            spriteBatch.DrawString(arial, "Time", new Vector2(730, 20), Color.White);

            // Draw time remaining with appropriate number of leading 0s
            if (secondsRemaining >= 100) spriteBatch.DrawString(arial, secondsRemaining.ToString("F0"), new Vector2(740, 50), Color.White);
            else if (secondsRemaining >= 10) spriteBatch.DrawString(arial, "0" + secondsRemaining.ToString("F0"), new Vector2(740, 50), Color.White);
            else if (secondsRemaining >= 0) spriteBatch.DrawString(arial, "00" + secondsRemaining.ToString("F0"), new Vector2(740, 50), Color.White);
            else spriteBatch.DrawString(arial, "000", new Vector2(740, 50), Color.White); // Prevent displaying negative time

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CoinCollected()
        {
            coinsCollected += 1;
            if(coinsCollected >= 100)
            {
                coinsCollected -= 100;
                mario.IncrementLivesRemaining();
            }
        }
    }
}

