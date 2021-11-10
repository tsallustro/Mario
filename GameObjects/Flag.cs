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
        private float timeLimit = 50f * 0.016666667f;
        private bool isDown = false;
        private bool isDescending = false;

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
            timer += (float)GameTime.ElapsedGameTime.TotalSeconds;

            if (isColliding)
            {
                if (timer >= timeLimit)
                {
                    Sprite = spriteFactory.CreateFlag(GetPosition());
                    isColliding = false;
                    isDown = true;
                } else if (!isDescending)
                {
                    isDescending = true;
                    Sprite = spriteFactory.CreateEndingFlag(GetPosition());
                }
            } else
            {
                Sprite = spriteFactory.CreateFlag(GetPosition());
            }

            Sprite.Update();
        }

        public override void Collision(int side, GameObject Collidee)
        {
            if (Collidee is Mario && !isDown)
            {
                isColliding = true;
            }
        }

        public override void ResetObject()
        {
            // Do nothing
        }
    }
}
