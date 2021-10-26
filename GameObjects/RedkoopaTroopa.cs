using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using States;
using Sprites;
using Factories;
using View;

namespace GameObjects
{
    public class RedKoopaTroopa : GameObject, IEnemy
    {
        private readonly int boundaryAdjustment = 4;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */
        private readonly int numberOfSpritesOnSheet = 3;
        private IEnemyState redKoopaTroopaState;
        private RedKoopaTroopaSpriteFactory spriteFactory;

        //Timer for Koopa Shell NOTE:CtrlV 
        private float timer;
        private float shellSpeed;
        Camera camera;

        public RedKoopaTroopa(Vector2 position, Camera camera)
            : base(position, new Vector2(0, 0), new Vector2(0, 0))
        {
            spriteFactory = RedKoopaTroopaSpriteFactory.Instance;
            Sprite = spriteFactory.CreateIdleRedKoopaTroopa(position);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            redKoopaTroopaState = new IdleRedKoopaTroopaState(this);
            this.camera = camera;
        }

        public IEnemyState GetRedKoopaTroopaState()
        {
            return this.redKoopaTroopaState;
        }

        public void SetRedKoopaTroopaState(IEnemyState redKoopaTroopaState)
        {
            this.redKoopaTroopaState = redKoopaTroopaState;
        }

        public override void Halt()
        {
            this.SetXVelocity(0);
            this.SetYVelocity(0);
        }

        public override void Damage()
        {
            redKoopaTroopaState.Stomped();
        }

        //This is a copy paste from KoopaTroopa
        public override void Collision(int side, GameObject Collidee)
        {
            const int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;

            if (Collidee is Mario)
            {
                if (redKoopaTroopaState is StompedRedKoopaTroopaState)
                {
                    switch (side)
                    {
                        case TOP:
                            redKoopaTroopaState.Stomped();
                            break;
                        case BOTTOM:
                            //Do nothing. Not sure what happens when Mario hits the shell from bottom.
                            break;
                        case LEFT:
                            //shell is kicked.
                            shellSpeed = 100;
                            Kicked(shellSpeed);
                            break;
                        case RIGHT:
                            shellSpeed = -100;
                            Kicked(shellSpeed);
                            break;
                    }
                }
                else
                {
                    if (side == 1)          //Top
                    {
                        redKoopaTroopaState.Stomped();
                    }
                }
            }
            else if (Collidee is KoopaTroopa koopa) //If koopa is also shelled and kicked, then it only changes direction when it hits another kicked koopa. If it's in any other state, another kicked koopa kills it.
            {
                if (koopa.GetKoopaTroopaState() is MovingShelledKoopaTroopaState && this.GetRedKoopaTroopaState() is MovingShelledKoopaTroopaState)
                {
                    this.SetXVelocity(this.GetVelocity().X * -1);
                }
                else
                {
                    this.Die();
                }
            }
            else if (Collidee is Block)  //Koopa changes its direction when it hits block
            {
                if (this.GetRedKoopaTroopaState() is MovingShelledKoopaTroopaState)
                {
                    this.SetXVelocity(this.GetVelocity().X * -1);
                }
            }
            else if (Collidee is FireBall && ((FireBall)Collidee).getActive())
            {
                this.Damage();
            }
        }


        //Update all of Goomba's members
        public override void Update(GameTime gameTime)
        {
            if (this.Position.X - camera.Position.X < 800)
            {
                redKoopaTroopaState.Move();
            }
            Sprite = spriteFactory.GetCurrentSprite(Sprite.location, redKoopaTroopaState);
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
            redKoopaTroopaState.Stomped();
        }

        //Change Goomba state to moving mode
        public void Move()
        {
            redKoopaTroopaState.Move();
        }

        //Change Goomba state to idle mode
        public void StayIdle()
        {
            redKoopaTroopaState.StayIdle();
        }

        public void Kicked(float sspeed)
        {
            timer = 50;
            SetXVelocity(sspeed);
            SetRedKoopaTroopaState(new MovingRedShelledKoopaTroopaState(this));
        }
        public void Die()
        {
            SetRedKoopaTroopaState(new DeadRedKoopaTroopaState(this));
        }
    }
}
