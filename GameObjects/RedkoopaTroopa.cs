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
    public class RedKoopaTroopa : GameObject, IEnemy
    {
        private readonly float gravityAccleration = 275;
        private readonly int boundaryAdjustment = 4;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */
        private readonly int numberOfSpritesOnSheet = 5;
        private readonly float timeInShell = 7;
        private IEnemyState redKoopaTroopaState;
        private RedKoopaTroopaSpriteFactory spriteFactory;
        private GameObject BlockEnemyIsOn { get; set; }

        //Timer for Koopa Shell
        private float timer;
        List<IGameObject> objects;
        private bool left = true;
        Camera camera;

        public RedKoopaTroopa(Vector2 position, Camera camera)
            : base(position, new Vector2(0, 0), new Vector2(0, 0))
        {
            timer = 0;
            spriteFactory = RedKoopaTroopaSpriteFactory.Instance;
            Sprite = spriteFactory.CreateIdleRedKoopaTroopa(position);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            redKoopaTroopaState = new IdleRedKoopaTroopaState(this);
            this.camera = camera;
            this.Acceleration = new Vector2(0, gravityAccleration);
        }

        public IEnemyState GetRedKoopaTroopaState()
        {
            return this.redKoopaTroopaState;
        }
        public void SetRedKoopaTroopaState(IEnemyState redKoopaTroopaState)
        {
            this.redKoopaTroopaState = redKoopaTroopaState;
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
            redKoopaTroopaState.Stomped();
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
                        redKoopaTroopaState.Stomped();
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
                if (!(this.redKoopaTroopaState is MovingRedKoopaTroopaState) && !(this.redKoopaTroopaState is MovingShelledKoopaTroopaState) && !(this.redKoopaTroopaState is IdleRedKoopaTroopaState) && !(this.redKoopaTroopaState is DeadRedKoopaTroopaState))
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
                }
                else if (redKoopaTroopaState is MovingRedShelledKoopaTroopaState)
                {
                    mario.Damage();
                }
            }
            else if (Collidee is KoopaTroopa koopa) //If koopa is also shelled and kicked, then it only changes direction when it hits another kicked koopa. If it's in any other state, another kicked koopa kills it.
            {
                if (koopa.GetKoopaTroopaState() is MovingShelledKoopaTroopaState && this.GetRedKoopaTroopaState() is MovingRedShelledKoopaTroopaState)
                {
                    this.SetXVelocity(this.GetVelocity().X * -1);
                }
                else if (koopa.GetKoopaTroopaState() is MovingRedShelledKoopaTroopaState)
                {
                    this.Die();
                }
            }
            else if (Collidee is Goomba goomba && redKoopaTroopaState is MovingRedShelledKoopaTroopaState)
            {
                goomba.Damage();
            }
            else if (Collidee is Block)  //Koopa changes its direction when it hits block
            {
                if (this.GetRedKoopaTroopaState() is MovingRedShelledKoopaTroopaState)
                {
                    this.SetXVelocity(this.GetVelocity().X * -1);
                }
            }
            else if (Collidee is FireBall && ((FireBall)Collidee).getActive())
            {
                this.Damage();
            }
        }

        //Update all of Koopa's members
        public override void Update(GameTime GameTime)
        {
            float timeElapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;
            timer += timeElapsed;

            //800 would be the width of viewport. koopa's move once in the view
            if (this.Position.X - camera.Position.X < 800)
            {
                redKoopaTroopaState.Move();
            }

            if (timer >= timeInShell && !(redKoopaTroopaState is MovingRedShelledKoopaTroopaState))
            {
                redKoopaTroopaState = new IdleRedKoopaTroopaState(this);
                timer = 0;
            }

            Velocity += (Acceleration * timeElapsed);
            Position = Position + (Velocity * timeElapsed);
            base.Update(GameTime);

            //If Goomba is not standing on anything, it should fall
            if (BlockEnemyIsOn != null && !BottomCollision(BlockEnemyIsOn))
            {
                this.SetXVelocity(this.GetVelocity().X * -1);
            }

            Sprite = spriteFactory.GetCurrentSprite(Position, redKoopaTroopaState);

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
            timer = 0;
            this.redKoopaTroopaState.Stomped();
            this.SetXVelocity(0);
            //timer = 50;
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
        public void Revive()
        {
            if (redKoopaTroopaState is StompedKoopaTroopaState)
            {
                if (timer == 0)
                {
                    redKoopaTroopaState.Move();
                } else
                {
                    timer--;
                }
            }
        }
        public void Kicked(float sspeed)
        {
            redKoopaTroopaState.Kicked(sspeed);
        }
            
        public void Die()
        {
            this.SetRedKoopaTroopaState(new DeadRedKoopaTroopaState(this));
        }

        public bool IsDead()
        {
            if (this.GetRedKoopaTroopaState() is DeadKoopaTroopaState)
                return true;
            return false;
        }
    }
}
