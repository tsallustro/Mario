﻿using Microsoft.Xna.Framework;
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
        private readonly bool canWarp;
        private Vector2 warpedPosition;

        public WarpPipe(Vector2 position, Vector2 velocity, Vector2 acceleration) : base(position, velocity, acceleration)
        {
            AABB = new Rectangle((int) position.X,(int) position.Y, 32, 64);
            canWarp = false;
        }

        public WarpPipe(Vector2 position, Vector2 velocity, Vector2 acceleration, bool canWarp, Vector2 warpedPosition) : base(position, velocity, acceleration)
        {
            AABB = new Rectangle((int)position.X, (int)position.Y, 32, 64);
            this.canWarp = canWarp;
            this.warpedPosition = warpedPosition;
        }

        public Vector2 GetWarpPosition()
        {
            return warpedPosition;
        }

        public bool CanWarp()
        {
            return canWarp;
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
