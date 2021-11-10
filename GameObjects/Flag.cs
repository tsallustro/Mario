using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
using Collisions;
using Factories;

namespace GameObjects
{
    class Flag : GameObject
    {
        private readonly int numberOfSpritesOnSheet = 50;
        private FlagSpriteFactory spriteFactory;
        private int end = 0;
        private bool isColliding = false;
        private float timer = 0;
        private float timeForFlagToDescend = 85f * 0.016667f; // This is based on nothing but experimentation...
        private bool isDown = false;

        public Flag(Vector2 position, Vector2 velocity, Vector2 acceleration) : base(position, velocity, acceleration)
        {
            spriteFactory = FlagSpriteFactory.Instance;
            Sprite = spriteFactory.CreateFlag(position);
            AABB = new Rectangle((int)position.X + 20, (int)position.Y, (Sprite.texture.Width / numberOfSpritesOnSheet) - 10, Sprite.texture.Height);
        }

        public override void Damage()
        {
            //Pipes cannot be damaged
            return;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, false);
            DrawAABBIfVisible(Color.Blue, spriteBatch);
        }

        public override void Halt()
        {
            return;
        }

        public override void Update(GameTime GameTime)
        {
            if (isColliding && !isDown)
            {
                timer += (float)GameTime.ElapsedGameTime.TotalSeconds;
                Sprite = spriteFactory.CreateEndingFlag(GetPosition());

                if (timer > timeForFlagToDescend)
                {
                    isColliding = false;
                    isDown = true;
                }
            }
            else if (!isDown) Sprite = spriteFactory.CreateFlag(GetPosition());
            else Sprite = spriteFactory.CreateFinalFlag(GetPosition());

            Sprite.Update();
        }

        public override void Collision(int side, GameObject Collidee)
        {
            if (Collidee is Mario && !isDown)
            {
                isColliding = true;
            } else
            {
                isColliding = false;
            }
        }

        public override void ResetObject()
        {
            // Do nothing
        }
    }
}
