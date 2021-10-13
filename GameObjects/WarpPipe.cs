using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
using Collisions;
namespace GameObjects
{
    class WarpPipe : GameObject
    {
        public WarpPipe(Vector2 position, Vector2 velocity, Vector2 acceleration) : base(position, velocity, acceleration)
        {
            
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
          
        }
    }
}
