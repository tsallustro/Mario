// Maxwell Ortwig

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using Controllers;
using OUpdater;
using Commands;
using GameObjects;
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
        
        // this object holds flags that tell objects whether or not they need to update
        private ObjectUpdater objectUpdater;
       
        // These are the four sprites used displayed in this game.
        private ISprite fixedSprite;
        private ISprite fixedAnimatedSprite;
        private ISprite movingSprite;
        private ISprite movingAnimatedSprite;

        private Mario mario;

        public MarioGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // create the ObjectUpdater and controllers
            objectUpdater = new ObjectUpdater();
            keyboardController = new KeyboardController();
            gamepadController = new GamepadController();
            this.Window.Title = "Sprint0";
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            arialSpriteFont = Content.Load<SpriteFont>("ArialSpriteFont");
            Texture2D marioSpriteSheet = Content.Load<Texture2D>("MarioSpriteSheet");

            // initialize the 4 sprites to be used with the parameters wanted for initial launch
            fixedSprite = new FixedSprite(objectUpdater, true, new Vector2(50, 25), marioSpriteSheet, 1, 4);
            fixedAnimatedSprite = new FixedAnimatedSprite(objectUpdater, true, new Vector2(150, 25), marioSpriteSheet, 1, 4);
            movingSprite = new MovingSprite(objectUpdater, true, new Vector2(250, 25), marioSpriteSheet, 1, 4);
            movingAnimatedSprite = new MovingAnimatedSprite(objectUpdater, true, new Vector2(350, 25), marioSpriteSheet, 1, 4);
            
            // Initialize keyboard controller mappings
            keyboardController.AddMapping((int)Keys.Q, new QuitCommand(this));
            keyboardController.AddMapping((int)Keys.W, new NonMovingNonAnimatedCommand(fixedSprite));
            keyboardController.AddMapping((int)Keys.E, new NonMovingAnimatedCommand(fixedAnimatedSprite));
            keyboardController.AddMapping((int)Keys.R, new MovingNonAnimatedCommand(movingSprite));
            keyboardController.AddMapping((int)Keys.T, new MovingAnimatedCommand(movingAnimatedSprite));
           
            // Initialize gamepad controller mappings
            gamepadController.AddMapping((int)Buttons.Start, new QuitCommand(this));
            gamepadController.AddMapping((int)Buttons.A, new NonMovingNonAnimatedCommand(fixedSprite));
            gamepadController.AddMapping((int)Buttons.B, new NonMovingAnimatedCommand(fixedAnimatedSprite));
            gamepadController.AddMapping((int)Buttons.X, new MovingNonAnimatedCommand(movingSprite));
            gamepadController.AddMapping((int)Buttons.Y, new MovingAnimatedCommand(movingAnimatedSprite));
        }
        protected override void Update(GameTime gameTime)
        {
            // Update the controllers
            gamepadController.Update();
            keyboardController.Update();

            // update the sprites
            fixedSprite.Update();
            fixedAnimatedSprite.Update();
            movingSprite.Update();
            movingAnimatedSprite.Update();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            // call draw methods from each sprite and pass in sprite batch
            fixedSprite.Draw(spriteBatch);
            fixedAnimatedSprite.Draw(spriteBatch);
            movingSprite.Draw(spriteBatch);
            movingAnimatedSprite.Draw(spriteBatch);
            // Draw Legend
            spriteBatch.DrawString(arialSpriteFont, "Controls (Keyboard/Gamepad)\nQ/start: Quit\nW/A:Toggle Visibility of fixedSprite\nE/B:Toggle Visibility of fixedAnimatedSprite\nR/X:Toggle Visibility of movingSprite\nT/Y:Toggle Visibility of movingAnimatedSprite", new Vector2(25, 325), Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }


}
