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
        private readonly int HorizontalVelocity = 50;
        private readonly int boundaryAdjustment = -10;
        private Camera camera;


        private Boolean left;   // Direction the missile is facing/going
        private Boolean active; // Active determines how this object collides and if it is visible

        private GameObject sender;

        public Missile(Boolean Left, GameObject Sender, Camera camera)       // Constructer for missile
           : base(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 250))
        {
            left = Left;
            active = true;
            sender = Sender;
            this.camera = camera;

            if (left)       // Set X Velocity based on direction
            {
                this.SetXVelocity(HorizontalVelocity);
            }
            else
            {
                this.SetXVelocity(-1 * HorizontalVelocity);
            }
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
            this.SetQueuedForDeletion(true);
        }
        public override void Halt()
        {
            this.SetXVelocity(0);
            this.SetYVelocity(0);
        }

        public void ThrowMissile()
        {
            System.Diagnostics.Debug.WriteLine("Missile Thrown");

            this.Position = sender.GetPosition();
            if (left)
                this.Position = new Vector2(this.Position.X - 50, this.Position.Y);
            else
                this.Position = new Vector2(this.Position.X + 50, this.Position.Y);
            this.active = true;
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
                System.Diagnostics.Debug.WriteLine("Missile Collided");
                if (Collidee is Mario)          // We're only concerned about colliding with Mario
                {
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
                this.Position += Velocity * timeElapsed;

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
                Sprite.Draw(spriteBatch, true);
                DrawAABBIfVisible(Color.Red, spriteBatch);
            }
        }

        public override void ResetObject()
        {
            this.SetQueuedForDeletion(true);
        }
    }
}