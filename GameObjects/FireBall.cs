using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Factories;
using States;

namespace GameObjects
{
    public class FireBall : GameObject
    {
        private FireBallSpriteFactory spriteFactory;
        private readonly int numberOfSpritesOnSheet = 4;
        private readonly int HorizontalVelocity = 100;
        private readonly int boundaryAdjustment = 8;


        private Boolean left;   // Direction the fireball is facing/going
        private Boolean active; // Active determines how this object collides and if it is visible
        private FireBall nextFireBall;

        private Mario mario;

        public FireBall(Boolean Left, Mario Mario)       // Constructer for fireball
           : base(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 150))
        {
            left = Left;
            active = false;
            mario = Mario;
            if (left)       // Set X Velocity based on direction
            {
                this.SetXVelocity(HorizontalVelocity);
            }
            else
            {
                this.SetXVelocity(-1 * HorizontalVelocity);
            }
            spriteFactory = FireBallSpriteFactory.Instance;
            Sprite = spriteFactory.CreateFireBall(this.Position);
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
            // Do nothing
        }
        public override void Halt()
        {
            this.SetXVelocity(0);
            this.SetYVelocity(0);
        }

        public void setNextFireBall(FireBall FireBall)
        {
            nextFireBall = FireBall;
        }

        public void ThrowFireBall()
        {
            if (mario.GetPowerState() is FireMario)
            {
                if (!this.active)
                {
                    this.active = true;
                    if (mario.isFacingLeft())
                    {
                        this.Position = new Vector2 (mario.GetPosition().X - 16, mario.GetPosition().Y);
                        this.SetXVelocity(-1 * HorizontalVelocity);
                    } else
                    {
                        this.Position = new Vector2(mario.GetPosition().X + 16, mario.GetPosition().Y);
                        this.SetXVelocity(HorizontalVelocity);
                    }
                    Sprite = spriteFactory.GetCurrentSprite(this.Position);
                    AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                        (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));

                }
                else if (nextFireBall != null && !nextFireBall.active)
                {
                    nextFireBall.ThrowFireBall();
                }
            }
        }


        // Updating Methods
        public override void Collision(int side, GameObject Collidee)
        {
            const int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;
            
            if (this.active) // only handle collisions if active
            {
                if (Collidee is Block && ((Block)Collidee).GetBlockState() is HiddenBlockState)
                {
                    return;     //Short circuit that! we dont care about hidden blocks
                }

                if (side == 1)          //Top
                {
                    this.active = false;
                }
                else if (side == 2)     //Bottom
                {
                    if (Collidee is Block)
                    {
                        this.SetYVelocity(-100);
                    } else
                    {
                        this.active = false;
                    }
                }
                else if (side == 3)     //Left
                {
                    this.active = false;
                }
                else if (side == 4)     //Right
                {
                    this.active = false;
                }
            }
        }

        public override void Update(GameTime GameTime)
        {
            if (this.active)
            {
                float timeElapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;

                // Move Fireball
                this.SetYVelocity(this.Velocity.Y + Acceleration.Y * timeElapsed);
                Position += Velocity * timeElapsed;

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
                Sprite = spriteFactory.GetCurrentSprite(Position); // uses same sprite to draw both fireballs so must reset sprite here, will change soon, TODO
                Sprite.Draw(spriteBatch, true);
                DrawAABBIfVisible(Color.Red, spriteBatch);
            }
        }



    }
}