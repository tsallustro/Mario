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

        public bool ContinueRunning { get; set; } = false;
        private bool JumpIsHeld { get; set; } = false;
        private float TimeJumpHeld { get; set; } = 0;
        private Block BlockMarioIsOn { get; set; }
        GraphicsDeviceManager Graphics { get; set; }

        // Physics variables
        private int MaxHorizontalSpeed { get; } = 150;
        private int FallingAcceleration { get; } = 155; // Must be consistent across files
        private int JumpHoldAccelerationBoost { get; } = 40;

        public Mario(Vector2 position, Vector2 velocity, Vector2 acceleration, GraphicsDeviceManager graphics, Point maxCoords)
            : base(position, velocity, acceleration)
        {
            spriteFactory = MarioSpriteFactory.Instance;
            Sprite = spriteFactory.CreateStandardIdleMario(position);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2), 
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            
            powerState = new StandardMario(this);
            actionState = new FallingState(this, false);
            Graphics = graphics;

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

            // Update the position of the sprite to account for varying heights of Mario
            if ((this.powerState is StandardMario || this.powerState is DeadMario) && 
                !(powerState is StandardMario) && !(powerState is DeadMario))
            {
                Position -= new Vector2(0, this.Sprite.texture.Height);
            } else if ((!(this.powerState is StandardMario) && !(this.powerState is DeadMario))
                && powerState is StandardMario)
            {
                Position += new Vector2(0, this.Sprite.texture.Height / 2);
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

        public override void Update(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Handle jump timer to allow higher jumping when button held
            if (actionState is JumpingState && JumpIsHeld) TimeJumpHeld += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // If at max speed, only change Y velocity
            if (Velocity.X < MaxHorizontalSpeed && Velocity.X > -MaxHorizontalSpeed)
                Velocity += Acceleration * timeElapsed;
            else
                SetYVelocity(Velocity.Y + Acceleration.Y * timeElapsed);

            base.Update(gameTime);

            newPosition = Position + Velocity * timeElapsed;
            CheckAndHandleIfAtScreenBoundary();
            Position = newPosition;

            // Fall if we are at peak of jump and not currently on a block
            if (BlockMarioIsOn != null && !BottomCollision(BlockMarioIsOn) && this.Velocity.Y >= 0)
            {
                this.actionState.Fall();
            }

            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            Sprite.Update();
        }

        public override void Collision(int side, GameObject obj)
        {
            const int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;

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
            else if (obj is Block block)
            {
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
                                this.actionState.Land();
                            }

                            BlockMarioIsOn = block;

                            break;
                        case LEFT:
                            if (Velocity.X < 0)
                            {
                                this.SetYVelocity(0);
                                this.SetXAcceleration(0);
                                this.SetXVelocity(0);
                                //this.actionState.Fall();
                            }
                            break;
                        case RIGHT:
                            if (Velocity.X > 0)
                            {
                                this.SetYVelocity(0);
                                this.SetXAcceleration(0);
                                this.SetXVelocity(0);
                                //this.actionState.Fall();
                            }
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
                        // Here, we reset the position slightly so the character can move away
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
    

        private void CheckAndHandleIfAtScreenBoundary()
        {
            //This prevents Mario from going outside the screen
            if (newPosition.X > maxCoords.X)
            {
                this.SetXVelocity(0);
                this.Acceleration = new Vector2(0, FallingAcceleration);
            }
            else if (newPosition.X < 0)
            {
                this.SetXVelocity(0);
                this.Acceleration = new Vector2(0, FallingAcceleration);
            }
            if (newPosition.Y > maxCoords.Y)
            {
                newPosition = new Vector2(Position.X, maxCoords.Y);
                if (!(powerState is DeadMario)) powerState = new DeadMario(this);
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
            if (actionState is FallingState)
            {
                if (pressType == 1 || pressType == 2) ContinueRunning = true;
                else ContinueRunning = false;
            }

            if (!(powerState is DeadMario) && !(actionState is FallingState) && !(actionState is JumpingState) && !(actionState is CrouchingState))
            {
                if (pressType == 1 || pressType == 2) actionState.MoveLeft();
                else actionState.MoveRight();
            } else if (actionState is FallingState || actionState is JumpingState || actionState is CrouchingState)
            {
                actionState.MoveLeft();
            }

            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
        }

        public void MoveRight(int pressType)
        {
            if (actionState is FallingState)
            {
                if (pressType == 1 || pressType == 2) ContinueRunning = true;
                else ContinueRunning = false;
            }

            if (!(powerState is DeadMario) && !(actionState is FallingState) && !(actionState is JumpingState) && !(actionState is CrouchingState))
            {
                if (pressType == 1 || pressType == 2) actionState.MoveRight();
                else actionState.MoveLeft();
            }
            else if (actionState is FallingState || actionState is JumpingState || actionState is CrouchingState)
            {
                actionState.MoveRight();
            }

            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
        }

        public void Up(int pressType)
        {
            if (!(powerState is DeadMario) && (pressType == 1 || pressType == 2))
            {
                JumpIsHeld = true;

                if (Velocity.Y == 0)
                    actionState.Jump();
                else if (TimeJumpHeld > 0.4 && Acceleration.Y >= FallingAcceleration - JumpHoldAccelerationBoost)
                    SetYAcceleration(Acceleration.Y - JumpHoldAccelerationBoost);
            }
            else if (pressType == 3)
            {
                TimeJumpHeld = 0;
                JumpIsHeld = false;
                SetYAcceleration(FallingAcceleration);
            }

            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
        }

        public void Down(int pressType)
        {
            if (!(powerState is DeadMario)) actionState.Crouch();
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
