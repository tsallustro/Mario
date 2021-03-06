using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using States;
using Sprites;
using Factories;
using Cameras;

namespace GameObjects
{
    public class KoopaTroopa : GameObject, IEnemy
    {
        private readonly float gravityAccleration = 275;
        private readonly int boundaryAdjustment = 4;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */
        private readonly int numberOfSpritesOnSheet = 5;
        private readonly float timeInShell = 7;
        private IEnemyState koopaTroopaState;
        private KoopaTroopaSpriteFactory spriteFactory;
        private GameObject BlockEnemyIsOn { get; set; }

        //Timer for Koopa Shell
        private float timer;
        List<IGameObject> objects;
        private bool left = true;
        Camera camera;

        public KoopaTroopa(Vector2 position, Camera camera)
            : base(position, new Vector2(0, 0), new Vector2(0, 0))
        {
            // Save initial Data
            resetState.pos = position;
            resetState.vel = new Vector2(0, 0);
            resetState.acc = new Vector2(0, gravityAccleration);

            timer = 0;
            spriteFactory = KoopaTroopaSpriteFactory.Instance;
            Sprite = spriteFactory.CreateIdleKoopaTroopa(position);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            koopaTroopaState = new IdleKoopaTroopaState(this);
            this.camera = camera;
            this.Acceleration = new Vector2(0, gravityAccleration);
        }
        public override void ResetObject()
        {
            this.Position = resetState.pos;
            this.Velocity = resetState.vel;
            this.Acceleration = resetState.acc;

            timer = 0;
            Sprite = spriteFactory.CreateIdleKoopaTroopa(resetState.pos);
            AABB = (new Rectangle((int)resetState.pos.X + (boundaryAdjustment / 2), (int)resetState.pos.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            koopaTroopaState = new IdleKoopaTroopaState(this);

            left = true;
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
        //Handle Collision with Block
        private void HandleBlockCollision(int side, Block block)
        {
            const int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;
            switch (side)
            {
                case TOP:
                    //Do Nothing
                    break;
                case BOTTOM:
                    if (!(block.GetBlockState() is HiddenBlockState))
                    {
                        this.SetYVelocity(0);
                        this.SetYAcceleration(0);
                    }
                    else if (block.GetBumped())
                    {
                        koopaTroopaState.Stomped();
                    }

                    BlockEnemyIsOn = block;
                    break;
                case LEFT:
                    if (!(block.GetBlockState() is HiddenBlockState))
                    {
                        this.SetXVelocity(this.GetVelocity().X * -1);
                    }
                    break;
                case RIGHT:
                    if (!(block.GetBlockState() is HiddenBlockState))
                    {
                        this.SetXVelocity(this.GetVelocity().X * -1);
                    }
                    break;
            }
        }
        //KoopaTroopa is affected only when Mario stomps it. Everything affects the other object.
        public override void Collision(int side, GameObject Collidee)
        {
            const int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;

            if (Collidee is Block block)
            {
                HandleBlockCollision(side, block);
            }
            else if (Collidee is WarpPipe)
            {
                if (side == LEFT || side == RIGHT)
                {
                    this.SetXVelocity(this.GetVelocity().X * -1);
                }
            }
            else if (Collidee is FireBall)
            {
                this.Damage();
                return;
            }
            else if (Collidee is Mario mario)
            {
                if (!(this.koopaTroopaState is MovingKoopaTroopaState) && !(this.koopaTroopaState is MovingShelledKoopaTroopaState) && !(this.koopaTroopaState is IdleKoopaTroopaState) && !(this.koopaTroopaState is DeadKoopaTroopaState))
                {
                    switch (side)
                    {
                        case TOP:
                            Stomped();
                            break;
                        case BOTTOM:
                            //Do nothing. Not sure what happens when Mario hits the shell from bottom.
                            break;
                        case LEFT:
                            Position = new Vector2(this.Position.X + 10, this.Position.Y);
                            Kicked(mario.GetVelocity().X + 50);
                            break;
                        case RIGHT:
                            Position = new Vector2(this.Position.X - 10, this.Position.Y);
                            Kicked(mario.GetVelocity().X - 50);

                            break;
                    }
                }
                else if (side == TOP)
                {
                    Stomped();
                } else if (koopaTroopaState is MovingShelledKoopaTroopaState && Math.Abs(this.Velocity.X) > 0)
                {
                    //mario.Damage();
                }
            }
            else if (Collidee is KoopaTroopa koopa) //If koopa is also shelled and kicked, then it only changes direction when it hits another kicked koopa. If it's in any other state, another kicked koopa kills it.
            {
                if (koopa.GetKoopaTroopaState() is MovingShelledKoopaTroopaState && this.GetKoopaTroopaState() is MovingShelledKoopaTroopaState) {
                    this.SetXVelocity(this.GetVelocity().X * -1);
                } else if (koopa.GetKoopaTroopaState() is MovingShelledKoopaTroopaState)
                {
                    this.Die();
                }
            } else if (Collidee is Goomba goomba && koopaTroopaState is MovingShelledKoopaTroopaState)
            {
                goomba.Damage();
            } else if (Collidee is Block)  //Koopa changes its direction when it hits block
            {
                if (this.GetKoopaTroopaState() is MovingShelledKoopaTroopaState)    
                {
                    this.SetXVelocity(this.GetVelocity().X * -1);
                }
            } /*else if (Collidee is FireBall && ((FireBall)Collidee).getActive())
            {
                this.Damage();
            }*/
        }

        //Update all of Koopa's members
        public override void Update(GameTime GameTime)
        {
            float timeElapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;
            timer += timeElapsed;

            //800 would be the width of viewport. koopa's move once in the view
            if (this.Position.X - camera.Position.X < 800)
            {
                koopaTroopaState.Move();
            }

            if (timer >= timeInShell && !(koopaTroopaState is MovingShelledKoopaTroopaState))
            {
                koopaTroopaState = new IdleKoopaTroopaState(this);
                timer = 0;
            }

            Velocity += (Acceleration * timeElapsed);
            Position = Position + (Velocity * timeElapsed);
            base.Update(GameTime);

            //If Goomba is not standing on anything, it should fall
            if (BlockEnemyIsOn != null && !BottomCollision(BlockEnemyIsOn))
            {
                this.SetYAcceleration(gravityAccleration);
            }

            Sprite = spriteFactory.GetCurrentSprite(Position, koopaTroopaState);

            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, (Sprite.texture.Height) - boundaryAdjustment));
            Sprite.Update();
        }

        //Draw koopaTroopa
        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.location = Position;
            Sprite.Draw(spriteBatch, left);
            DrawAABBIfVisible(Color.Red, spriteBatch);
        }

        //Change koopaTroopa state to stomped mode
        public void Stomped()
        {
            timer = 0;
            this.koopaTroopaState.Stomped();
            this.SetXVelocity(0);
            //timer = 50;
        }

        //Change koopaTroopa state to moving mode
        public void Move()
        {
            koopaTroopaState.Move();
        }
        //Change koopaTroopa state to idle mode
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
            koopaTroopaState.Kicked(sspeed);
        }
            
        public void Die()
        {
            this.SetKoopaTroopaState(new DeadKoopaTroopaState(this));
        }

        public bool IsDead()
        {
            if (this.GetKoopaTroopaState() is DeadKoopaTroopaState)
                return true;
            return false;
        }
    }
}
