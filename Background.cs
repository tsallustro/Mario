using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Game1;
using GameObjects;
using Sprites;
using States;
using Cameras;

namespace View
{

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
        private Mario mario;

        private Camera camera;
        private RenderTarget2D renderTarget1 = null;
        static Vector2 VirtualScreen = new Vector2(1400, 800);
        Vector2 parallax;

        private float timer = 1;

        private Vector2 savePoint;
        private Vector2 firstHalfScreen;
        private bool passedFirstHalfScreen = false;

        //Y location of background bushes and hills.
        private float backgroundYPos;

        public Background(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, MarioGame game, Mario mario, Camera camera)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            this.game = game;
            this.mario = mario;
            this.camera = camera;
            //Background sprite height = 40, Block height = 16
            this.backgroundYPos = graphicsDevice.Viewport.Height - 40 - 32;
            firstHalfScreen = new Vector2(0, graphicsDevice.Viewport.Y + ((graphicsDevice.Viewport.Height) / 2));
            savePoint = firstHalfScreen;
            
        }

        public float GetBottomBoundary()
        {
            return graphicsDevice.Viewport.Y + graphicsDevice.Viewport.Height;
        }

        public void SetCamera(Camera camera)
        {
            this.camera = camera;
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
        public void Update(GameTime gameTime)
        {
            if (mario.GetPosition().Y > (camera.Position.Y + 480))
            {
                mario.SetPowerState(new DeadMario(mario));
            }
            
            //ScrollNormal();
            ScrollUp();
            //ScrollAuto(gameTime);
        }

        //Three Methods how to scroll camera.
        //camera scroll follows Mario position
        public void ScrollNormal()
        {
            camera.LookAt(new Vector2(0, mario.GetPosition().Y));
        }
        //camera scrolls only upward, not downward
        public void ScrollUp()
        {
            if(passedFirstHalfScreen == false && firstHalfScreen.Y > mario.GetPosition().Y)
            {
                passedFirstHalfScreen = true;
            }
            if (passedFirstHalfScreen == false)
            {
                camera.LookAt(new Vector2(0, mario.GetPosition().Y));
            } else
            {
                if (camera.Position.Y + ((graphicsDevice.Viewport.Height) / 2) < mario.GetPosition().Y)
                {
                    camera.LookAt(savePoint);
                } else
                {
                    camera.LookAt(new Vector2(0, mario.GetPosition().Y));
                    savePoint = mario.GetPosition();
                }
            }
        }
        //camera automatically scrolls upward nonstop until the edge.
        public void ScrollAuto(GameTime gameTime)
        {
            float totalGametime = (float)gameTime.TotalGameTime.TotalSeconds;
            float startScrollTime = 5;
            if (totalGametime > startScrollTime)
            {
                camera.LookAt(new Vector2(0, firstHalfScreen.Y - 10 * (totalGametime - startScrollTime)));
            }
            
        }
        public void Draw()
        {
            //DrawCloud(renderTarget1);
            if (renderTarget1 == null)
            {
                renderTarget1 = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
                graphicsDevice.SetRenderTarget(renderTarget1);
                spriteBatch.Begin();
                graphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Draw(cloudThree, new Vector2(50, 220), Color.White);
                spriteBatch.Draw(cloudThree, new Vector2(300, 100), Color.White);
                spriteBatch.Draw(cloudTwo, new Vector2(200, 300), Color.White);
                spriteBatch.Draw(cloudTwo, new Vector2(600, 150), Color.White);
                spriteBatch.Draw(cloudOne, new Vector2(340, 110), Color.White);
                spriteBatch.End();
                graphicsDevice.SetRenderTarget(null);
            }
            Vector2 parallax = new Vector2(0.7f);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, camera.GetViewMatrix(parallax));
            spriteBatch.Draw((Texture2D)renderTarget1, Vector2.Zero, new Rectangle(0, 0, (int)VirtualScreen.X, (int)VirtualScreen.Y), Color.White);
            spriteBatch.End();
        }

        //Draw clouds
        public void DrawCloud(RenderTarget2D renderTarget)
        {
            if (renderTarget == null)
            {
                renderTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
                graphicsDevice.SetRenderTarget(renderTarget);
                spriteBatch.Begin();
                graphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Draw(cloudThree, new Vector2(50, 220), Color.White);
                spriteBatch.Draw(cloudThree, new Vector2(300, 100), Color.White);
                spriteBatch.Draw(cloudTwo, new Vector2(200, 300), Color.White);
                spriteBatch.Draw(cloudTwo, new Vector2(600, 150), Color.White);
                spriteBatch.Draw(cloudOne, new Vector2(340, 110), Color.White);
                spriteBatch.End();
                graphicsDevice.SetRenderTarget(null);
            }
            Vector2 parallax = new Vector2(0.7f);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, camera.GetViewMatrix(parallax));
            spriteBatch.Draw((Texture2D)renderTarget, Vector2.Zero, new Rectangle(0, 0, (int)VirtualScreen.X, (int)VirtualScreen.Y), Color.White);
            spriteBatch.End();
        }
        //Draw Bushes
        public void DrawBush(RenderTarget2D renderTarget)
        {

            if (renderTarget == null)
            {
                renderTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
                graphicsDevice.SetRenderTarget(renderTarget);
                spriteBatch.Begin();
                graphicsDevice.Clear(Color.Transparent);
                spriteBatch.Draw(bushOne, new Vector2(30, backgroundYPos), Color.White);
                spriteBatch.Draw(bushTwo, new Vector2(250, backgroundYPos), Color.White);
                spriteBatch.Draw(bushTwo, new Vector2(580, backgroundYPos), Color.White);
                spriteBatch.Draw(bushThree, new Vector2(360, backgroundYPos), Color.White);
                spriteBatch.Draw(bushOne, new Vector2(670, backgroundYPos), Color.White);
                spriteBatch.Draw(bushTwo, new Vector2(1320, backgroundYPos), Color.White);
                spriteBatch.End();
                graphicsDevice.SetRenderTarget(null);
            }
            parallax = new Vector2(0.8f);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, camera.GetViewMatrix(parallax));
            spriteBatch.Draw((Texture2D)renderTarget, Vector2.Zero, new Rectangle(0, 0, (int)VirtualScreen.X, (int)VirtualScreen.Y), Color.White);
            spriteBatch.End();
            renderTarget = null;
        }
        //Draw Hill
        public void DrawHill(RenderTarget2D renderTarget)
        {
            if (renderTarget == null)
            {
                renderTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
                graphicsDevice.SetRenderTarget(renderTarget);
                spriteBatch.Begin();
                graphicsDevice.Clear(Color.Transparent);
                spriteBatch.Draw(hillSmallSprite, new Vector2(220, backgroundYPos), Color.White);
                spriteBatch.Draw(hillBigSprite, new Vector2(600, backgroundYPos), Color.White);
                spriteBatch.Draw(hillSmallSprite, new Vector2(650, backgroundYPos), Color.White);
                spriteBatch.Draw(hillBigSprite, new Vector2(1300, backgroundYPos), Color.White);
                spriteBatch.End();
                graphicsDevice.SetRenderTarget(null);
            }
            parallax = new Vector2(0.7f);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, camera.GetViewMatrix(parallax));
            spriteBatch.Draw((Texture2D)renderTarget, Vector2.Zero, new Rectangle(0, 0, (int)VirtualScreen.X, (int)VirtualScreen.Y), Color.White);
            spriteBatch.End();
            renderTarget = null;
        }
    }
}
