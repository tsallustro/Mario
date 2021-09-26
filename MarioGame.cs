// Maxwell Ortwig

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using Controllers;
using OUpdater;
using Commands;
namespace Game1
{
    public class MarioGame : Game
    {
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
            this.Window.Title = "Cornet Mario Game";

            base.Initialize();
        }

        protected override void LoadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
            // Update the controllers
            gamepadController.Update();
            keyboardController.Update();

            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
         

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }


}
