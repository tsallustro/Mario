using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Game1;

namespace View
{
    class Camera
    {
        public Camera(Viewport viewport)
        {
            Origin = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
            Zoom = 1.0f;
        }

        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public float Zoom { get; set; }
        public float Rotation { get; set; }

        public Matrix GetViewMatrix(Vector2 parallax)
        {
            // To add parallax, simply multiply it by the position
            return Matrix.CreateTranslation(new Vector3(-Position * parallax, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }
    }
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
        public Background(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, MarioGame game)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            this.game = game;
        }
        public void LoadContent()
        {
            hillSmallSprite = game.Content.Load<Texture2D>("hillSmall");
            hillBigSprite = game.Content.Load<Texture2D>("hillBig");
            cloudOne = game.Content.Load<Texture2D>("cloud1");
            cloudTwo = game.Content.Load<Texture2D>("cloud2");
            cloudThree = game.Content.Load<Texture2D>("cloud3");
            bushOne = game.Content.Load<Texture2D>("bush1");
            bushTwo = game.Content.Load<Texture2D>("bush2");
            bushThree = game.Content.Load<Texture2D>("bush3");
        }

        public void Draw()
        {
            Camera camera = new Camera(graphicsDevice.Viewport);
            Vector2 parallax = new Vector2(0.8f);   //hills pass by at the slightly slower speed
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(parallax));
            spriteBatch.Draw(hillSmallSprite, new Vector2(100, graphicsDevice.Viewport.Height - 40), Color.White);
            spriteBatch.Draw(hillBigSprite, new Vector2(300, graphicsDevice.Viewport.Height - 50), Color.White);
            spriteBatch.End();
            parallax = new Vector2(0.3f);   //clouds pass by slowest
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(parallax));
            spriteBatch.Draw(cloudOne, new Vector2(50, 200), Color.White);
            spriteBatch.Draw(cloudTwo, new Vector2(200, 300), Color.White);
            spriteBatch.Draw(cloudThree, new Vector2(300, 150), Color.White);
            spriteBatch.End();
            parallax = new Vector2(1f);     //Bushes pass by at regular speed
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(parallax));
            spriteBatch.Draw(bushOne, new Vector2(30, graphicsDevice.Viewport.Height - 20), Color.White);
            spriteBatch.Draw(bushTwo, new Vector2(280, graphicsDevice.Viewport.Height - 20), Color.White);
            spriteBatch.Draw(bushThree, new Vector2(160, graphicsDevice.Viewport.Height - 20), Color.White);
            spriteBatch.End();
        }
    }
}
