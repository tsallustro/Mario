using Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;
using System.Collections.Generic;

namespace GameObjects
{
    public class Mario : GameObject, IAvatar
    {
        private readonly int boundaryAdjustment = 0;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */
        private readonly int numberOfSpritesOnSheet = 15;
        private IMarioPowerState powerState;
        private IMarioActionState actionState;
        private MarioSpriteFactory spriteFactory;
        private Point maxCoords;
        private Vector2 newPosition;
        private Vector2 oldPosition;
        List<IGameObject> objects;
        GraphicsDeviceManager Graphics { get; set; }

        public Mario(Vector2 position, Vector2 velocity, Vector2 acceleration, GraphicsDeviceManager graphics, Point maxCoords, List<IGameObject> objs)
            : base(position, velocity, acceleration)
        {
            spriteFactory = MarioSpriteFactory.Instance;
            Sprite = spriteFactory.CreateStandardIdleMario(position);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2), 
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            
            powerState = new StandardMario(this);
            actionState = new IdleState(this, false);
            Graphics = graphics;
            objects = objs;

            // Adjust given maxCoords to account for sprite's height
            this.maxCoords = new Point(maxCoords.X - (Sprite.texture.Width / numberOfSpritesOnSheet), maxCoords.Y - Sprite.texture.Height);
        }

        public IMarioPowerState GetPowerState()
        {
            return powerState;
        }

        public void SetPowerState(IMarioPowerState powerState)
        {
            int previousSpriteHeight = Sprite.texture.Height;
            if(this.powerState is StandardMario)
            {
                Position = Position - new Vector2(0, this.Sprite.texture.Height);

            } 
            this.powerState = powerState;

            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);

            // Update maxCoords to match change in height from power state
            maxCoords.Y -= (Sprite.texture.Height - previousSpriteHeight);
        }

       
        public void SetActionState(IMarioActionState actionState)
        {
            this.actionState = actionState;
        }

        public override void Update(GameTime GameTime)
        {

            float timeElapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;
            newPosition = Position + Velocity * timeElapsed;

            StopMarioBoundary();
            Position = newPosition;

            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            Sprite.Update();
        }

        public override void Collision(int side, GameObject obj)
        {
            const int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;
            IBlockState blockState;

            if (obj is Item item)
            {
                if (item is SuperMushroom)
                {
                    this.powerState.Mushroom();
                }
                else if (item is FireFlower)
                {
                    this.powerState.FireFlower();
                }
                else if (item is Star)
                {
                    //Implement invicibility
                }
            }
            else if (obj is Block)
            {
                Block block = (Block)obj;
                if (!(block.GetBlockState() is HiddenBlockState))
                {
                    switch (side)
                    {
                        case TOP:
                            if (Velocity.Y < 0) this.actionState.Fall();
                            break;
                        case BOTTOM:
                            if (Velocity.Y > 0 && !(this.actionState is RunningState) && !(this.actionState is IdleState))
                            {
                                this.SetYVelocity(0);
                            }
                            break;
                        case LEFT:
                            if (Velocity.X < 0) this.actionState.Idle();
                            break;
                        case RIGHT:
                            if (Velocity.X > 0) this.actionState.Idle();
                            break;
                    }
                } else
                {
                    switch(side)
                    {
                        case TOP:
                            if (Velocity.Y < 0) this.actionState.Fall();
                            break;
                    }
                }
            }

            else if (obj is Goomba goomba)
            {
                if (!(goomba.GetGoombaState() is StompedGoombaState) && !(goomba.GetGoombaState() is DeadGoombaState))

                    switch (side)
                    {
                        case TOP:
                            this.powerState.TakeDamage();
                            this.actionState.Idle();
                            Position = new Vector2(Position.X, Position.Y - 2);
                            break;
                        case BOTTOM:
                            //Skip off of enemy
                            break;
                        case LEFT:
                            this.powerState.TakeDamage();
                            this.actionState.Idle();
                            Position = new Vector2(Position.X + 2, Position.Y);
                            break;
                        case RIGHT:
                            this.powerState.TakeDamage();
                            this.actionState.Idle();
                            Position = new Vector2(Position.X - 2, Position.Y);
                            break;
                    }
            }
        }
    

        private void StopMarioBoundary()
        {
            //This prevents Mario from going outside the screen
            if (newPosition.X > maxCoords.X)
            {
                newPosition = new Vector2(maxCoords.X, Position.Y);
            }
            else if (newPosition.X < 0)
            {
                newPosition = new Vector2(0, Position.Y);
            }
            if (newPosition.Y > maxCoords.Y)
            {
                newPosition = new Vector2(Position.X, maxCoords.Y);
            }
            else if (newPosition.Y < 0)
            {
                newPosition = new Vector2(Position.X, 0);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.location = Position;
            Sprite.Draw(spriteBatch, actionState.GetDirection());
            DrawAABBIfVisible(Color.Yellow, spriteBatch);
        }
        
        public void MoveLeft(int pressType)
        {
            
            /*
            if (this.actionState is RunningState && pressType == 2)
            {
                this.location.X -= 1;
            }
            */
            if (!(powerState is DeadMario)) actionState.MoveLeft();
            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
        }

        public void MoveRight(int pressType)
        {
            
            //Mario only moves when holding the key
            if (!(powerState is DeadMario)) actionState.MoveRight();
            //sprite.Move();
            /*if (this.actionState is RunningState && pressType == 2)
            {
                this.location.X += 1;
            }*/
            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
        }

        public void Up(int pressType)
        {
            if (!(powerState is DeadMario)) actionState.Jump();
            /*if (this.actionState is JumpingState && pressType == 2)
            {
                this.location.Y -= 1;
            }/*
            /*
             * actionState.Jump() already changes the state to idle if crouching or to jumping if idle
             * Is there need to do it again, besides changing velocity?
            if (this.actionState is CrouchingState)
            {
                this.SetActionState(new IdleState(this, actionState.GetDirection()));
                velocity.Y = 0;
            } else
            {
                velocity.Y = 100;
                this.SetActionState(new IdleState(this, actionState.GetDirection()));
                actionState.Jump();
            }
            */
            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
        }

        public void Down(int pressType)
        {
            if (!(powerState is DeadMario)) actionState.Crouch();
            /*if (this.actionState is CrouchingState && pressType == 2)
            {
                this.location.Y += 1;
            }*/
            /* UNCOMMENT FOR SPRINT2
            if (this.actionState is JumpingState || this.actionState is FallingState)
            {
                this.SetActionState(new IdleState(this, actionState.GetDirection()));
                velocity.Y = 0;
            }
            */
            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
        }

        public override void Damage() { }

        public override void Halt()
        {
            /*foreach (GameObject obj in objects)
            {
                if (obj != this)
                {
                    if (obj is Block)
                    {

                        if ((this.RightCollision(obj) && this.Velocity.X > 0) || (this.LeftCollision(obj) && this.Velocity.X < 0))
                        {
                            this.actionState.Idle();
                        } else if (this.TopCollision(obj) && this.Velocity.Y < 0)
                        {
                            this.actionState.Fall();
                        } else if (this.BottomCollision(obj) && this.Velocity.Y > 0)
                        {
                            this.actionState.Land();
                        }
                    } 
                }
            }*/
        }
        public bool CanBreakBricks()
        {
            return !(powerState is StandardMario || powerState is DeadMario);
        }
        public bool isFacingLeft()
        {
            return actionState.GetDirection();
        }

        public void Die()
        {
            oldPosition = Position;
            this.powerState.TakeDamage();
            this.SetXVelocity((float)0);
            this.SetYVelocity((float)-20);
            if (Position.Y < oldPosition.Y - (float)5 && Velocity.Y < 0)
            {
                this.SetYVelocity((float)20);
            } else if (Position.Y > oldPosition.Y + (float)5)
            {
                Sprite.ToggleVisibility();
            }
        }
    }
}
