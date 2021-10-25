using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using States;
using Sprites;
using Factories;

namespace GameObjects
{
    public class KoopaTroopa : GameObject, IEnemy
    {
        private readonly int boundaryAdjustment = 4;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */
        private readonly int numberOfSpritesOnSheet = 5;
        private IEnemyState koopaTroopaState;
        private KoopaTroopaSpriteFactory spriteFactory;

        //Timer for Koopa Shell
        private float timer;
        private float shellSpeed;
        List<IGameObject> objects;
        private bool left = true;

        Vector2 newPosition;

        public KoopaTroopa(Vector2 position)
            : base(position, new Vector2(0, 0), new Vector2(0, 0))
        {
            if (koopaTroopaState is StompedKoopaTroopaState)
            {
                Revive();
            }
            spriteFactory = KoopaTroopaSpriteFactory.Instance;
            Sprite = spriteFactory.CreateIdleKoopaTroopa(position);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            koopaTroopaState = new IdleKoopaTroopaState(this);
        }

        public IEnemyState GetKoopaTroopaState()
        {
            return this.koopaTroopaState;
        }
        public void SetKoopaTroopaState(IEnemyState koopaTroopaState)
        {
            this.koopaTroopaState = koopaTroopaState;
        }
        public bool GetDirection()
        {
            if (this.GetVelocity().X <= 0 )
            {
                left = true;
            } else
            {
                left = false;
            }
            return left;
        }

        public override void Halt()
        {
            this.SetXVelocity(0);
            this.SetYVelocity(0);
        }

        public override void Damage()
        {
            koopaTroopaState.Stomped();
        }

        //KoopaTroopa is affected only when Mario stomps it. Everything affects the other object.
        public override void Collision(int side, GameObject Collidee)
        {
            const int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;

            if (Collidee is Mario)
            {
                if (this.koopaTroopaState is StompedKoopaTroopaState)
                {
                    
                    switch (side)
                    {
                        case TOP:
                            koopaTroopaState.Stomped();
                            break;
                        case BOTTOM:
                            //Do nothing. Not sure what happens when Mario hits the shell from bottom.
                            break;
                        case LEFT:
                            this.SetYVelocity(-100);
                            //shell is kicked.
                            shellSpeed = 100;
                            Kicked(shellSpeed);
                            break;
                        case RIGHT:
                            shellSpeed = -100;
                            Kicked(shellSpeed);
                            break;
                    }
                } else 
                {
                    if (side == TOP)
                    {
                        koopaTroopaState.Stomped();
                    } 
                }
            } else if (Collidee is KoopaTroopa koopa) //If koopa is also shelled and kicked, then it only changes direction when it hits another kicked koopa. If it's in any other state, another kicked koopa kills it.
            {
                if (koopa.GetKoopaTroopaState() is MovingShelledKoopaTroopaState && this.GetKoopaTroopaState() is MovingShelledKoopaTroopaState)     {
                    this.SetXVelocity(this.GetVelocity().X * -1);
                } else
                {
                    this.Die();
                }
            } else if (Collidee is Block)  //Koopa changes its direction when it hits block
            {
                if (this.GetKoopaTroopaState() is MovingShelledKoopaTroopaState)    
                {
                    this.SetXVelocity(this.GetVelocity().X * -1);
                }
            }
        }

        //Update all of Goomba's members
        public override void Update(GameTime GameTime)
        {
            float timeElapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;
            Position = Position + (Velocity * timeElapsed);
            base.Update(GameTime);
            Sprite = spriteFactory.GetCurrentSprite(Position, koopaTroopaState);
            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, (Sprite.texture.Height) - boundaryAdjustment));
            Sprite.Update();
        }

        //Draw Goomba
        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.location = Position;
            Sprite.Draw(spriteBatch, left);
            DrawAABBIfVisible(Color.Red, spriteBatch);
        }

        //Change Goomba state to stomped mode
        public void Stomped()
        {
            koopaTroopaState.Stomped();
            this.SetXVelocity(0);
            timer = 50;
        }

        //Change Goomba state to moving mode
        public void Move()
        {
            koopaTroopaState.Move();
        }
        //Change Goomba state to idle mode
        public void StayIdle()
        {
            koopaTroopaState.StayIdle();
        }
        public void Revive()
        {
            if (koopaTroopaState is StompedKoopaTroopaState)
            {
                if (timer == 0)
                {
                    koopaTroopaState.Move();
                } else
                {
                    timer--;
                }
            }
        }
        public void Kicked(float sspeed)
        {
            timer = 50;
            koopaTroopaState.Kicked(sspeed);
        }
            
        public void Die()
        {
            SetKoopaTroopaState(new DeadKoopaTroopaState(this));
        }
    }
}
