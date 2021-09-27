// Maxwell Ortwig

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using Controllers;
using Commands;
using GameObjects;
using Factories;

namespace Game1
{
    public class MarioGame : Game
    {
        static private MarioGame _sprint;

        public static MarioGame sprint
        {
            get
            {
                return _sprint;
            }
        }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont arialSpriteFont;

        // controllers that detect input from gamepad and keyboard. These only change ObjectUpdater
        private IController keyboardController;
        private IController gamepadController;

        // Simple display of sprites for sprint1
        private ISprite flower;
        private ISprite coin;
        private ISprite mushroom;
        private ISprite oneUpMushroom;
        private ISprite star;
        private ISprite stairBlock;
        private ISprite usedBlock;
        private ISprite questionBlock;
        private ISprite brickBlock;
        private ISprite hiddenBlock;
        //private ISprite goomba;
        private ISprite koopaTroopa;

        private Goomba goomba;
        private GoombaSpriteFactory goombaSpriteFactory;
        private Mario mario;
        private MarioSpriteFactory marioSpriteFactory;

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

            this.Window.Title = "Sprint0";
            _sprint = this;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            arialSpriteFont = Content.Load<SpriteFont>("ArialSpriteFont");
            Texture2D marioSpriteSheet = Content.Load<Texture2D>("MarioSpriteSheet");

            marioSpriteFactory.LoadTextures(this);
            goombaSpriteFactory.LoadTextures(this);

            //Visuals for Sprint 1
            flower = new Flower(true, sprint, new Vector2(50, 50));
            coin = new Coin(true, sprint, new Vector2(100, 50));
            mushroom = new Mushroom(true, sprint, new Vector2(150, 50));
            oneUpMushroom = new MushroomOneUp(true, sprint, new Vector2(200, 50));
            star = new Star (true, sprint, new Vector2(250, 50));
            stairBlock = new StairBlock(true, sprint, new Vector2(50, 150));
            usedBlock = new UsedBlock(true, sprint, new Vector2(100, 150));
            questionBlock = new QuestionBlock(true, sprint, new Vector2(150, 150));
            brickBlock = new BrickBlock(true, sprint, new Vector2(200, 150));
            hiddenBlock = new HiddenBlock(true, sprint, new Vector2(250, 150));
            //goomba = new Goomba(true, sprint, new Vector2(300, 100));
            koopaTroopa = new KoopaTroopa(true, sprint, new Vector2(350, 100));

            mario = new Mario();
            goomba = new Goomba();

            // Initialize commands that will be repeated
            ICommand moveLeft = new MoveLeftCommand(mario);
            ICommand moveRight = new MoveRightCommand(mario);
            ICommand jump = new JumpCommand(mario);
            ICommand crouch = new CrouchCommand(mario);

            // Initialize keyboard controller mappings
            keyboardController.AddMapping((int)Keys.Q, new QuitCommand(this));
            keyboardController.AddMapping((int)Keys.Left, moveLeft);
            keyboardController.AddMapping((int)Keys.A, moveLeft);
            keyboardController.AddMapping((int)Keys.Right, moveRight);
            keyboardController.AddMapping((int)Keys.D, moveRight);
            keyboardController.AddMapping((int)Keys.Up, jump);
            keyboardController.AddMapping((int)Keys.W, jump);
            keyboardController.AddMapping((int)Keys.Down, crouch);
            keyboardController.AddMapping((int)Keys.S, crouch);
            keyboardController.AddMapping((int)Keys.Y, new StandardMarioCommand(mario));
            keyboardController.AddMapping((int)Keys.U, new SuperMarioCommand(mario));
            keyboardController.AddMapping((int)Keys.I, new FireMarioCommand(mario));
            keyboardController.AddMapping((int)Keys.O, new DeadMarioCommand(mario));

            keyboardController.AddMapping((int)Keys.B, new IdleGoombaCommand(goomba));
            keyboardController.AddMapping((int)Keys.N, new MovingGoombaCommand(goomba));
            keyboardController.AddMapping((int)Keys.M, new StompedGoombaCommand(goomba));

            // Initialize gamepad controller mappings
            gamepadController.AddMapping((int)Buttons.DPadLeft, moveLeft);
            gamepadController.AddMapping((int)Buttons.DPadRight, moveRight);
            gamepadController.AddMapping((int)Buttons.A, jump);
            gamepadController.AddMapping((int)Buttons.DPadDown, crouch);
        }
        protected override void Update(GameTime gameTime)
        {
            // Update the controllers
            gamepadController.Update();
            keyboardController.Update();

            // update the sprites
            mario.Update();

            goomba.Update();

            flower.Update();
            coin.Update();
            mushroom.Update();
            oneUpMushroom.Update();
            star.Update();
            stairBlock.Update();
            usedBlock.Update();
            questionBlock.Update();
            brickBlock.Update();
            hiddenBlock.Update();
            
            koopaTroopa.Update();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            // call draw methods from each sprite and pass in sprite batch
            mario.Draw(spriteBatch);

            goomba.Draw(spriteBatch);

            flower.Draw(spriteBatch, true);
            coin.Draw(spriteBatch, true);
            mushroom.Draw(spriteBatch, true);
            oneUpMushroom.Draw(spriteBatch, true);
            star.Draw(spriteBatch, true);
            stairBlock.Draw(spriteBatch, true);
            usedBlock.Draw(spriteBatch, true);
            questionBlock.Draw(spriteBatch, true);
            brickBlock.Draw(spriteBatch, true);
            hiddenBlock.Draw(spriteBatch, true);
            
            koopaTroopa.Draw(spriteBatch, true);

            // Draw Legend
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }


}
