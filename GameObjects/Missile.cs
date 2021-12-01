using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Factories;
using Game1;
using Cameras;

namespace GameObjects
{
    public class Missile : GameObject
    {
        private MissileSpriteFactory spriteFactory;
        private readonly int numberOfSpritesOnSheet = 4;
        private readonly int HorizontalVelocity = 110;
        private readonly int boundaryAdjustment = 0;
        private Camera camera;


        private Boolean left;   // Direction the missile is facing/going
        private Boolean active; // Active determines how this object collides and if it is visible

        private GameObject sender;

        public Missile(Boolean Left, GameObject Sender, Camera camera)       // Constructer for missile
           : base(new Vector2(0, 0), new Vector2(150, 0), new Vector2(50, -30))
        {
            left = Left;
            active = true;
            sender = Sender;
            this.camera = camera;

            
            spriteFactory = MissileSpriteFactory.Instance;
            Sprite = spriteFactory.CreateMissile(this.Position);
            AABB = (new Rectangle((int)Position.X, (int)Position.Y,
                (Sprite.texture.Width / numberOfSpritesOnSheet), Sprite.texture.Height));

        }

        // Get Methods
        public Boolean getActive()
        {
            return active;
        }


        // Modifying Methods
        public override void Damage()
        {
            active = false;
        }
        public override void Halt()
        {
            this.SetXVelocity(0);
            this.SetYVelocity(0);
        }

        public void ThrowMissile()
        {
            this.Position = sender.GetPosition();
            this.active = true;

            if (left)       // Set X Velocity based on direction
            {
                this.Position = new Vector2(sender.GetPosition().X - 16, sender.GetPosition().Y);
                this.SetXVelocity(HorizontalVelocity);
            }
            else
            {
                this.SetXVelocity(-1 * HorizontalVelocity);
            }
        }




        // Updating Methods
        private void CheckAndHandleIfAtScreenBoundary() // ripped from mario.cs, handles if missile goes outside of screen
        {
            if (Position.X - (Sprite.texture.Width / numberOfSpritesOnSheet) > camera.Position.X + 800)       // default screen height is 800 px
            {
                this.active = false;
            }
            else if (Position.X + (Sprite.texture.Width / numberOfSpritesOnSheet) < camera.Position.X)
            {
                this.active = false;
            }
            if (Position.Y > (camera.Position.Y + 480))       // default screen Width is 480 px
            {
                this.active = false;
            }
            else if (Position.Y < camera.Position.Y)
            {
                this.active = false;
            }
        }
        public override void Collision(int side, GameObject Collidee)
        {
            if (this.active)
            {
                if (Collidee is Mario)          // We're only concerned about colliding with Mario
                {
                    System.Diagnostics.Debug.WriteLine("Missile Collided");
                    this.Damage();
                }
            }
        }

        public override void Update(GameTime GameTime)
        {
            if (this.active)
            {
                float timeElapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;

                // Check if missile is out of bounds
                this.CheckAndHandleIfAtScreenBoundary();

                // Move missile
                if (left)
                {
                    this.SetXVelocity(this.Velocity.X + Acceleration.X * timeElapsed);
                } else {
                    this.SetXVelocity(this.Velocity.X - Acceleration.X * timeElapsed);
                }
                this.SetYVelocity(this.Velocity.Y + Acceleration.Y * timeElapsed);
                this.Position -= Velocity * timeElapsed;
                base.Update(GameTime);

                // Update Sprite
                Sprite = spriteFactory.GetCurrentSprite(Position);
                AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                        (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
                Sprite.Update();
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                Sprite = spriteFactory.GetCurrentSprite(Position); // TODO
                Sprite.Draw(spriteBatch, left);
                DrawAABBIfVisible(Color.Red, spriteBatch);
            }
        }

        public override void ResetObject()
        {
            this.SetQueuedForDeletion(true);
        }
    }
}