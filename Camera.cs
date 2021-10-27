using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Game1;
using GameObjects;
using Sprites;

namespace View
{
    public class Camera
    {
        public Camera(Viewport viewport)
        {
            _viewport = viewport;
            Origin = new Vector2(_viewport.Width / 2.0f, _viewport.Height / 2.0f);
            Zoom = 1.0f;
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;

                // If there's a limit set and there's no zoom or rotation clamp the position
                if (Limits != null && Zoom == 1.0f && Rotation == 0.0f)
                {
                    _position.X = MathHelper.Clamp(_position.X, Limits.Value.X, Limits.Value.X + Limits.Value.Width - _viewport.Width);
                    _position.Y = MathHelper.Clamp(_position.Y, Limits.Value.Y, Limits.Value.Y + Limits.Value.Height - _viewport.Height);
                }
            }
        }

        public Vector2 Origin { get; set; }

        public float Zoom { get; set; }

        public float Rotation { get; set; }


        public Rectangle? Limits
        {
            get
            {
                return _limits;
            }
            set
            {
                if (value != null)
                {
                    // Assign limit but make sure it's always bigger than the viewport
                    _limits = new Rectangle
                    {
                        X = value.Value.X,
                        Y = value.Value.Y,
                        Width = System.Math.Max(_viewport.Width, value.Value.Width),
                        Height = System.Math.Max(_viewport.Height, value.Value.Height)
                    };

                    // Validate camera position with new limit
                    Position = Position;
                }
                else
                {
                    _limits = null;
                }
            }
        }

        public Matrix GetViewMatrix(Vector2 parallax)
        {
            return Matrix.CreateTranslation(new Vector3(-Position * parallax, 0.0f)) *
                   Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom, Zoom, 1.0f) *
                   Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        public void LookAt(Vector2 position)
        {
            Position = position - new Vector2(_viewport.Width / 2.0f, _viewport.Height / 2.0f);
        }

        public void Move(Vector2 displacement, bool respectRotation = false)
        {
            if (respectRotation)
            {
                displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(-Rotation));
            }

            Position += displacement;
        }

        private readonly Viewport _viewport;
        private Vector2 _position;
        private Rectangle? _limits;
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
        private GameObject mario;

        private Camera camera;
        
        public Background(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, MarioGame game, GameObject mario, Camera camera)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            this.game = game;
            this.mario = mario;
            this.camera = camera;
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
            spriteBatch.Draw(cloudTwo, new Vector2(200, 300),Color.White);
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
            spriteBatch.Draw(hillBigSprite, new Vector2(220, graphicsDevice.Viewport.Height - 40 - 32), Color.White);
            spriteBatch.Draw(hillBigSprite, new Vector2(600, graphicsDevice.Viewport.Height -40 - 32), Color.White);
            spriteBatch.Draw(hillSmallSprite, new Vector2(650, graphicsDevice.Viewport.Height - 40 - 32), Color.White);
            spriteBatch.Draw(hillBigSprite, new Vector2(1300, graphicsDevice.Viewport.Height - 40 - 32), Color.White);
            spriteBatch.Draw(hillSmallSprite, new Vector2(1550, graphicsDevice.Viewport.Height - 40 - 32), Color.White);
            spriteBatch.Draw(hillSmallSprite, new Vector2(2000, graphicsDevice.Viewport.Height - 40 - 32), Color.White);
            spriteBatch.Draw(hillBigSprite, new Vector2(2050, graphicsDevice.Viewport.Height - 40 - 32), Color.White);
            spriteBatch.End();

            parallax = new Vector2(0.8f);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, camera.GetViewMatrix(parallax));
            spriteBatch.Draw(bushOne, new Vector2(30, graphicsDevice.Viewport.Height -40 -32), Color.White);
            spriteBatch.Draw(bushTwo, new Vector2(250, graphicsDevice.Viewport.Height -40 -32), Color.White);
            spriteBatch.Draw(bushTwo, new Vector2(580, graphicsDevice.Viewport.Height - 40 - 32), Color.White);
            spriteBatch.Draw(bushThree, new Vector2(360, graphicsDevice.Viewport.Height -40 -32), Color.White);
            spriteBatch.Draw(bushOne, new Vector2(670, graphicsDevice.Viewport.Height - 40 - 32), Color.White);
            spriteBatch.Draw(bushTwo, new Vector2(1320, graphicsDevice.Viewport.Height - 40 - 32), Color.White);
            spriteBatch.Draw(bushThree, new Vector2(1730, graphicsDevice.Viewport.Height - 40 - 32), Color.White);
            spriteBatch.Draw(bushTwo, new Vector2(2400, graphicsDevice.Viewport.Height - 40 - 32), Color.White);
            spriteBatch.End();
        }
    }
}
