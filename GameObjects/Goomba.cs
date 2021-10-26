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
    public class Goomba : GameObject, IEnemy
    {
        private readonly float gravityAcceleration = 275;
        private readonly int boundaryAdjustment = 4;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */
        private GameObject BlockEnemyIsOn { get; set; }
        private readonly int numberOfSpritesOnSheet = 3;
        private IEnemyState goombaState;
        private GoombaSpriteFactory spriteFactory;
        private bool introduced = false;
        Vector2 newPosition;
        List<IGameObject> objects;
        Camera camera;

        //I
        public Goomba(Vector2 position, Vector2 velocity, Vector2 acceleration, List<IGameObject> objs, Camera camera)
            : base(position, velocity, acceleration)
        {
            spriteFactory = GoombaSpriteFactory.Instance;
            Sprite = spriteFactory.CreateIdleGoomba(position);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            goombaState = new IdleGoombaState(this);
            objects = objs;
            this.camera = camera;
            this.Acceleration = new Vector2(0, gravityAcceleration);
        }
        //Get Goomba State
        public IEnemyState GetGoombaState()
        {
            return this.goombaState;
        }
        //Set Goomba State
        public void SetGoombaState(IEnemyState goombaState)
        {
            this.goombaState = goombaState;
        }

        public override void Halt()
        {
            //Do Nothing
        }
        public override void Damage()
        {
            goombaState.Stomped();
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
                    } else if (block.GetBumped())
                    {
                        goombaState.Stomped();
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
        //Handles Collision with other Objects
        public override void Collision(int side, GameObject Collidee)
        {
            const int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;

            //Goomba should fall if not working on the 
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
            else if (Collidee is Mario)
            {
                if (side == TOP)
                {
                    this.Damage();
                }
            }
            else if (Collidee is KoopaTroopa koopaTroopa && koopaTroopa.GetKoopaTroopaState() is MovingShelledKoopaTroopaState)
            {
                this.Damage();
            } 
        }
        //If Goomba is not on blocks, it will have downward acceleration
        public void Gravity()
        {
            this.SetYVelocity(50);
        }


        //Update all of Goomba's members
        public override void Update(GameTime GameTime)
        {
            float timeElapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;
            //800 would be the width of the viewport
            if (introduced == false && this.Position.X - camera.Position.X < 800)
            {
                goombaState.Move();
                introduced = true;
            }

            Velocity += (Acceleration * timeElapsed);
            newPosition = Position + (Velocity * timeElapsed);
            Position = newPosition;

            //If Goomba is not standing on anything, it should fall
            if (BlockEnemyIsOn != null && !BottomCollision(BlockEnemyIsOn))
            {
                this.SetYVelocity(50);
            }

            Sprite = spriteFactory.GetCurrentSprite(Position, goombaState);
            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, (Sprite.texture.Height) - boundaryAdjustment));
            Sprite.Update();
        }

        //Draw Goomba
        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.location = Position;
            Sprite.Draw(spriteBatch, true);
            DrawAABBIfVisible(Color.Red, spriteBatch);
        }

        //Change Goomba state to stomped mode
        public void Stomped()
        {
            goombaState.Stomped();
        }

        //Change Goomba state to moving mode
        public void Move()
        {
            goombaState.Move();
        }
        public void ChangeDirection()
        {
            //Change direction when it reaches certain point
            if (this.Position.X > 700)
            {
                this.SetXVelocity((float)-50);
            } else if (this.Position.X <= 0)
            {
                this.SetXVelocity((float)50);
            }
        }

        //Change Goomba state to idle mode
        public void StayIdle()
        {
            goombaState.StayIdle();
        }

        

    }
}
