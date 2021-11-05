using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
using Collisions;
namespace GameObjects
{
    class Flag : GameObject
    {
        private readonly int numberOfSpritesOnSheet = 1;

        public Flag(Vector2 position, Vector2 velocity, Vector2 acceleration, ISprite sprite) : base(position, velocity, acceleration)
        {
            Sprite = sprite;
            AABB = new Rectangle((int)position.X, (int)position.Y, (Sprite.texture.Width / numberOfSpritesOnSheet), Sprite.texture.Height);
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
            //Warp pipe doesn't change
            return;
        }
        public override void Collision(int side, GameObject Collidee)
        {
            /*if (side == CollisionHandler.TOP || side == CollisionHandler.BOTTOM)
            {
                Collidee.SetYVelocity(0);
            }
            else
                Collidee.SetXVelocity(0);*/
        }
    }
}
