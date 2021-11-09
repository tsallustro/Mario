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

        public Flag(Vector2 position, Vector2 velocity, Vector2 acceleration, ISprite sprite) : base(position, velocity, acceleration)
        {
            spriteFactory = FlagSpriteFactory.Instance;
            Sprite = sprite;
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

            Sprite.Update();
            //Warp pipe doesn't change
        }
        public override void Collision(int side, GameObject Collidee)
        {
            /*if (side == CollisionHandler.TOP || side == CollisionHandler.BOTTOM)
            {
                Collidee.SetYVelocity(0);
            }
            else
                Collidee.SetXVelocity(0);*/
            if (Collidee is Mario && end == 0)
            {
                /*
                this.Sprite = spriteFactory.CreateEndingFlag(Position);
                end = 1;
                */
            }
        }

        public override void ResetObject()
        {
            // Do nothing
        }
    }
}
