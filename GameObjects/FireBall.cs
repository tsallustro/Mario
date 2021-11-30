using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Factories;
using States;
using Cameras;

namespace GameObjects
{
    public class FireBall : GameObject
    {
        private FireBallSpriteFactory spriteFactory;
        private readonly int numberOfSpritesOnSheet = 4;
        private readonly int HorizontalVelocity = 130;
        private readonly int boundaryAdjustment = 8;
        private Camera camera;


        private Boolean left;   // Direction the fireball is facing/going
        private Boolean active; // Active determines how this object collides and if it is visible
        private FireBall nextFireBall;

        private Mario mario;

        public FireBall(Boolean Left, Mario Mario, Camera camera)       // Constructer for fireball
           : base(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 250))
        {
            left = Left;
            active = false;
            mario = Mario;
            this.camera = camera;

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
                    this.SetYVelocity(-50);
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
        private void CheckAndHandleIfAtScreenBoundary() // ripped from mario.cs, handles if fireball goes outside of screen
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
            const int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;
            if (this.active)
            {

                if (Collidee is IItem || Collidee is FireBall || Collidee is Castle || Collidee is Flag) // Short on items and other fireballs
                {
                    // Do Nothing
                }
                else if (Collidee is Goomba || Collidee is KoopaTroopa || Collidee is RedKoopaTroopa || Collidee is Piranha)
                {   // for collision on enemies order matters, so enemy's dont collide with fireball (it is handled here instead)
                    if (((IEnemy)Collidee).IsDead())    // short on dead enemies
                    {
                        // Do Nothing
                        
                    }
                    else
                    {
                        ((IEnemy)Collidee).Stomped();
                        this.active = false;
                    }
                }
                else if (Collidee is Mario)
                {
                    mario.Damage();
                    this.active = false;
                }
                else                // do collision via sides (primarily for blocks)
                {
                    switch (side)
                    {
                        case TOP:
                            this.active = false;
                            break;

                        case BOTTOM:
                            if (Collidee is Block || Collidee is WarpPipe)      //bounce on blocks
                            {
                                this.SetYVelocity(-100);
                            }
                            else
                            {
                                this.active = false;
                            }
                            break;

                        case LEFT:
                            this.active = false;
                            break;

                        case RIGHT:
                            this.active = false;
                            break;
                    }
                }
            }

        }

        public override void Update(GameTime GameTime)
        {
            if (this.active)
            {
                float timeElapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;

                // Check if fireball is out of bounds
                this.CheckAndHandleIfAtScreenBoundary();

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

        public override void ResetObject()
        {
            // Do nothing
        }
    }
}