using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Game1;
using GameObjects;
using Sprites;
using Cameras;

namespace View
{

    //To get rid of clutters in MarioGame.cs
    //Sets the graphics using Camera
    class Background
    {
        //Background textures
        private Texture2D hillSmallSprite;
        private Texture2D hillBigSprite;
        private Texture2D cloudOne;
        private Texture2D cloudTwo;
        private Texture2D cloudThree;
        private Texture2D bushOne;
        private Texture2D bushTwo;
        private Texture2D bushThree;

        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;
        private MarioGame game;
        private GameObject mario;

        private Camera camera;

        //Y location of background bushes and hills.
        private float backgroundYPos;

        public Background(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, MarioGame game, GameObject mario, Camera camera)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            this.game = game;
            this.mario = mario;
            this.camera = camera;
            this.backgroundYPos = backgroundYPos = graphicsDevice.Viewport.Height - 40 - 32;
        }
        public void LoadContent()
        {
            //block size 16 by16
            //All have height of 40pt
            hillSmallSprite = game.Content.Load<Texture2D>("hillSmall");
            hillBigSprite = game.Content.Load<Texture2D>("hillBig");
            cloudOne = game.Content.Load<Texture2D>("cloud1");
            cloudTwo = game.Content.Load<Texture2D>("cloud2");
            cloudThree = game.Content.Load<Texture2D>("cloud3");
            bushOne = game.Content.Load<Texture2D>("bush1");
            bushTwo = game.Content.Load<Texture2D>("bush2");
            bushThree = game.Content.Load<Texture2D>("bush3");

        }
        public void Update()
        {
            camera.LookAt(new Vector2(mario.GetPosition().X, 0));
        }

        public void Draw()
        {
            Vector2 parallax = new Vector2(0.2f);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, camera.GetViewMatrix(parallax));
            spriteBatch.Draw(cloudThree, new Vector2(50, 220), Color.White);
            spriteBatch.Draw(cloudThree, new Vector2(300, 100), Color.White);
            spriteBatch.Draw(cloudTwo, new Vector2(200, 300), Color.White);
            spriteBatch.Draw(cloudTwo, new Vector2(600, 150), Color.White);
            spriteBatch.Draw(cloudOne, new Vector2(340, 110), Color.White);
            spriteBatch.Draw(cloudThree, new Vector2(550, 220), Color.White);
            spriteBatch.Draw(cloudThree, new Vector2(800, 105), Color.White);
            spriteBatch.Draw(cloudTwo, new Vector2(950, 290), Color.White);
            spriteBatch.Draw(cloudOne, new Vector2(120, 130), Color.White);
            spriteBatch.Draw(cloudOne, new Vector2(1250, 180), Color.White);
            spriteBatch.End();

            parallax = new Vector2(0.7f);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, camera.GetViewMatrix(parallax));
            spriteBatch.Draw(hillBigSprite, new Vector2(220, backgroundYPos), Color.White);
            spriteBatch.Draw(hillBigSprite, new Vector2(600, backgroundYPos), Color.White);
            spriteBatch.Draw(hillSmallSprite, new Vector2(650, backgroundYPos), Color.White);
            spriteBatch.Draw(hillBigSprite, new Vector2(1300, backgroundYPos), Color.White);
            spriteBatch.Draw(hillSmallSprite, new Vector2(1550, backgroundYPos), Color.White);
            spriteBatch.Draw(hillSmallSprite, new Vector2(2000, backgroundYPos), Color.White);
            spriteBatch.Draw(hillBigSprite, new Vector2(2050, backgroundYPos), Color.White);
            spriteBatch.End();

            parallax = new Vector2(0.8f);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, camera.GetViewMatrix(parallax));
            spriteBatch.Draw(bushOne, new Vector2(30, backgroundYPos), Color.White);
            spriteBatch.Draw(bushTwo, new Vector2(250, backgroundYPos), Color.White);
            spriteBatch.Draw(bushTwo, new Vector2(580, backgroundYPos), Color.White);
            spriteBatch.Draw(bushThree, new Vector2(360, backgroundYPos), Color.White);
            spriteBatch.Draw(bushOne, new Vector2(670, backgroundYPos), Color.White);
            spriteBatch.Draw(bushTwo, new Vector2(1320, backgroundYPos), Color.White);
            spriteBatch.Draw(bushThree, new Vector2(1730, backgroundYPos), Color.White);
            spriteBatch.Draw(bushTwo, new Vector2(2400, backgroundYPos), Color.White);
            spriteBatch.End();
        }
    }
}
