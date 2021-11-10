using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Cameras;
using Sprites;
using Collisions;
namespace GameObjects
{
    class WarpPipe : GameObject
    {
        private readonly bool canWarp;
        private Vector2 warpedPosition;
        private Camera camera;
        private double respawnTimer = 0;
        private bool objRevealable = true;
        protected IGameObject pipedObject;
        public WarpPipe(Vector2 position, Vector2 velocity, Vector2 acceleration, Camera Camera) : base(position, velocity, acceleration)
        {
            AABB = new Rectangle((int) position.X,(int) position.Y, 32, 64);
            canWarp = false;
            camera = Camera;
        }

        public WarpPipe(Vector2 position, Vector2 velocity, Vector2 acceleration, bool canWarp, Vector2 warpedPosition, Camera Camera) : base(position, velocity, acceleration)
        {
            AABB = new Rectangle((int)position.X, (int)position.Y, 32, 64);
            this.canWarp = canWarp;
            this.warpedPosition = warpedPosition;
            camera = Camera;
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
        public void setPipedObject(IGameObject gameObject)
        {
            pipedObject = gameObject;
        }

        public void revealObject()
        {
            float distance = Math.Abs(this.Position.X - (400 + camera.Position.X));
            // This controls whether mario is close enough to reveal an item or enemy

            if (pipedObject is IEnemy)
            {
                if (distance < 200 && distance > 60)
                {
                    System.Diagnostics.Debug.WriteLine("ENEMY REVEALING FROM PIPE");
                    System.Diagnostics.Debug.WriteLine(distance);
                    respawnTimer = 0;
                    pipedObject.UnPipe();
                }
            } else {
                if (distance < 50)
                {
                    System.Diagnostics.Debug.WriteLine("OBJECT REVEALING FROM PIPE");
                    System.Diagnostics.Debug.WriteLine(distance);
                    respawnTimer = 0;
                    pipedObject.UnPipe();
                }
            }
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
            // short if pipe doesnt have object
            if (pipedObject == null)
            {
                return;
            }
            
            if (respawnTimer < 5)                                       // If object is not revealable and the respawn timer is below 5s, count up!
            {
                pipedObject.SetIsPiped(true);
                respawnTimer += GameTime.ElapsedGameTime.TotalSeconds;
            } else {                                                    // if the object is revealable and the respawn timer is up, then call reveal
                this.revealObject();
            }
            
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

        public override void ResetObject()
        {
            // Do nothing
        }
    }
}
