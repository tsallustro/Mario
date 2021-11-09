using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using States;
using Sprites;
using Factories;
using Cameras;
using Sound;

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
        private readonly double deathTimer = 1.5; // Timer for stomped Goomba disappearing
        private double timeStomped = 0;

        private IEnemyState goombaState;
        private GoombaSpriteFactory spriteFactory;
        private bool introduced = false;
        Vector2 newPosition;
        List<IGameObject> objects;
        Camera camera;

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
            if (IsPiped)        // if goomba is a piped goomba, then we dont want to remove him  
            {
                this.Position = this.InitialPosition;
                return;
            }
            if (!(goombaState is StompedGoombaState) && !(goombaState is DeadGoombaState)) 
                Position = new Vector2(Position.X, Position.Y + 4);
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
                } else if (side == BOTTOM)
                {
                    this.SetYVelocity(0);
                    this.SetYAcceleration(0);
                }
            }
            else if (Collidee is Mario mario)
            {
                if (side == TOP && mario.GetVelocity().Y < 0)
                {
                    this.Damage();
                }
            }
            else if (Collidee is KoopaTroopa koopaTroopa && koopaTroopa.GetKoopaTroopaState() is MovingShelledKoopaTroopaState)
            {
                this.Damage();
            } 
        }

        //Update all of Goomba's members
        public override void Update(GameTime GameTime)
        {
            if (this.IsPiped)   // Short if goomba is piped
            {
                return;
            }

            float timeElapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;

            if (goombaState is StompedGoombaState || goombaState is DeadGoombaState)
            {
                timeStomped += timeElapsed;

                if (timeStomped >= deathTimer) this.queuedForDeletion = true;
            }

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
                this.SetYAcceleration(gravityAcceleration);
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

            if (!(goombaState is StompedGoombaState) && !(goombaState is DeadGoombaState))
                Position = new Vector2(Position.X, Position.Y + 4);
            goombaState.Stomped();
        }

        //Change Goomba state to moving mode
        public void Move()
        {
            goombaState.Move();
        }

        //Change Goomba state to idle mode
        public void StayIdle()
        {
            goombaState.StayIdle();
        }

        public bool IsDead()
        {
            if (this.GetGoombaState() is DeadGoombaState)
            {
                return true;
            } else {
                return false;
            }
        }
    }
}
