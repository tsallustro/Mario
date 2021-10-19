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
        private readonly int numberOfSpritesOnSheet = 3;
        private IEnemyState koopaTroopaState;
        private KoopaTroopaSpriteFactory spriteFactory;

        //Timer for Koopa Shell
        private float timer;
        private float shellSpeed;

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
                if (koopaTroopaState is StompedKoopaTroopaState)
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
                            //shell is kicked.
                            shellSpeed = 100;
                            Kicked();
                            break;
                        case RIGHT:
                            shellSpeed = -100;
                            Kicked();
                            break;
                    }
                } else 
                {
                    if (side == 1)          //Top
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
        public override void Update(GameTime gameTime)
        {
            Sprite = spriteFactory.GetCurrentSprite(Sprite.location, koopaTroopaState);
            Sprite.Update();
        }

        //Draw Goomba
        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, true);
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
        public void Kicked()
        {
            timer = 50;
            SetXVelocity(shellSpeed);
            SetKoopaTroopaState(new MovingShelledKoopaTroopaState(this));
        }
        public void Die()
        {
            SetKoopaTroopaState(new DeadKoopaTroopaState(this));
        }
    }
}
