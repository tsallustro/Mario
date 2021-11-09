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

        private int livesRemaining = 3;
       
        public bool hasWarped { get; set; } = false;

        const int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;

        public ScoreHandler score = new ScoreHandler();

        private IMarioPowerState powerState;
        private IMarioActionState actionState;
        private MarioSpriteFactory spriteFactory;
        private Point initialMaxCoords;
        private Point maxCoords;
        private Vector2 newPosition;
        private Vector2 oldPosition;

        public bool ContinueRunning { get; set; } = false;
        private bool JumpIsHeld { get; set; } = false;
        private float TimeJumpHeld { get; set; } = 0;
        private GameObject BlockMarioIsOn { get; set; }
        private IEnemy EnemyCollidedWith { get; set; }
        GraphicsDeviceManager Graphics { get; set; }

        private IMarioActionState previousAction { get; set; }

        // Physics variables
        private int MaxHorizontalSpeed { get; } = 120;
        private int FallingAcceleration { get; } = 275; // Must be consistent across files
        private int JumpHoldAccelerationBoost { get; } = 55;
        private float HighJumpTimer { get; } = 0.35f;

        public bool WinningStateReached { get; set; } = false;

        public Mario(Vector2 position, Vector2 velocity, Vector2 acceleration, GraphicsDeviceManager graphics, Point maxCoords)
            : base(position, velocity, acceleration)
        {
            spriteFactory = MarioSpriteFactory.Instance;
            Sprite = spriteFactory.CreateStandardIdleMario(position);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2), 
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            
            powerState = new StandardMario(this);
            actionState = new FallingState(this, false);
            previousAction = new FallingState(this, false);
            Graphics = graphics;

            // Adjust given maxCoords to account for sprite's height
            this.maxCoords = new Point(maxCoords.X - (Sprite.texture.Width / numberOfSpritesOnSheet), maxCoords.Y - Sprite.texture.Height);
            initialMaxCoords = this.maxCoords;
        }

        public int GetLivesRemaining()
        {
            return livesRemaining;
        }

        public void SetLivesRemaining(int livesRemaining)
        {
            this.livesRemaining = livesRemaining;
        }

        public void IncrementLivesRemaining()
        {
            livesRemaining++;
        }

        public void DecrementLivesRemaining()
        {
            livesRemaining--;
        }

        public IMarioPowerState GetPowerState()
        {
            return powerState;
        }
        public IMarioActionState GetActionState()
        {
            return actionState;
        }
        public IMarioActionState GetPreActionState()
        {
            return previousAction;
        }
        public void SetPosition(Vector2 position)
        {
            this.Position = position;
        }
        public void SetPowerState(IMarioPowerState PowerState)
        {
            int previousSpriteHeight = Sprite.texture.Height;

            // Update the position of the sprite to account for varying heights of Mario
            if ((this.powerState is StandardMario || this.powerState is DeadMario) && 
                !(PowerState is StandardMario) && !(PowerState is DeadMario))
            {
                Position -= new Vector2(0, this.Sprite.texture.Height);
            } else if ((!(this.powerState is StandardMario) && !(this.powerState is DeadMario))
                && PowerState is StandardMario)
            {
                Position += new Vector2(0, this.Sprite.texture.Height / 2);
            }

            this.powerState = PowerState;
            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, PowerState);

            // Update maxCoords to match change in height from power state
            maxCoords.Y -= (Sprite.texture.Height - previousSpriteHeight);
        }

       
        public void SetActionState(IMarioActionState actionState)
        {
            previousAction = this.actionState;
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

            /*
             * We need to keep track of which enemy Mario last collided with, so he doesn't continually
             * take damage and die immediately even when he is in Super or Fire state.
             */
            if (EnemyCollidedWith != null && !BottomCollision(EnemyCollidedWith) && !TopCollision(EnemyCollidedWith) &&
                !LeftCollision(EnemyCollidedWith) && !RightCollision(EnemyCollidedWith))
            {
                EnemyCollidedWith = null;
            }

            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            Sprite.Update();
        }

        private void HandleBlockCollision(int side, GameObject block)
        {
            switch (side)
            {
                case TOP:
                    if (Velocity.Y < 0)
                    {
                        this.actionState.Fall();
                    }

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
                        this.SetXAcceleration(0);
                        this.SetXVelocity(0);
                    }
                    break;
                case RIGHT:
                    if (Velocity.X > 0)
                    {
                        this.SetXAcceleration(0);
                        this.SetXVelocity(0);
                    }
                    break;
            }
        }

        public override void Collision(int side, GameObject Collidee)
        {
            if (Collidee is Item item && item.CanBePickedUp())
            {
                if (item is SuperMushroom)
                {
                    powerState.Mushroom();
                    score.IncreaseScore(1000);
                }
                else if (item is FireFlower)
                {
                    powerState.FireFlower();
                    score.IncreaseScore(1000);
                }
                else if (item is OneUpMushroom)
                {
                    IncrementLivesRemaining();
                }
                else if (item is Star)
                {
                    //Implement invicibility
                    score.IncreaseScore(1000);
                }
                
            }
            else if (Collidee is Block block)
            {
                if (!(block.GetBlockState() is HiddenBlockState))
                {
                    HandleBlockCollision(side, block);
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
            else if (Collidee is WarpPipe pipe)
            {
                HandleBlockCollision(side, pipe);
            } 
            else if (Collidee is Goomba goomba)
            {
                if (EnemyCollidedWith == null || (EnemyCollidedWith != null && goomba != EnemyCollidedWith))
                {
                    EnemyCollidedWith = goomba;
                    if (!(goomba.GetGoombaState() is StompedGoombaState) && !(goomba.GetGoombaState() is DeadGoombaState))
                    {
                        switch (side)
                        {
                            // Here, we reset the position slightly so the character can move away
                            case TOP:
                                this.Damage();
                                this.actionState.Idle();
                                Position = new Vector2(Position.X, Position.Y - 2);
                                break;
                            case BOTTOM:
                                //Skip off of enemy
                                actionState.Land();
                                actionState.Jump();
                                score.IncreaseScore(100);
                                break;
                            case LEFT:
                                this.Damage();
                                this.actionState.Idle();
                                break;
                            case RIGHT:
                                this.Damage();
                                this.actionState.Idle();
                                break;
                        }
                    }
                }
            }
            else if (Collidee is KoopaTroopa koopaTroopa)
            {
                if (EnemyCollidedWith == null || (EnemyCollidedWith != null && koopaTroopa != EnemyCollidedWith))
                {
                    EnemyCollidedWith = koopaTroopa;
                    if (!(koopaTroopa.GetKoopaTroopaState() is StompedKoopaTroopaState) && !(koopaTroopa.GetKoopaTroopaState() is DeadKoopaTroopaState) && !(koopaTroopa.GetKoopaTroopaState() is MovingShelledKoopaTroopaState))
                    {
                        switch (side)
                        {
                            // Here, we reset the position slightly so the character can move away
                            case TOP:
                                this.Damage();
                                this.actionState.Idle();
                                Position = new Vector2(Position.X, Position.Y - 2);
                                break;
                            case BOTTOM:
                                actionState.Land();
                                actionState.Jump();
                                score.IncreaseScore(100);
                                break;
                            case LEFT:
                                this.Damage();
                                this.actionState.Idle();
                                break;
                            case RIGHT:
                                this.Damage();
                                this.actionState.Idle();
                                break;
                        }
                    }
                    else if (!(koopaTroopa.GetKoopaTroopaState() is MovingGoombaState) && !(koopaTroopa.GetKoopaTroopaState() is DeadKoopaTroopaState))
                    {
                        switch (side)
                        {
                            case TOP:
                                //Does Mario gets damaged when he hits his head on Koopatroopa shell when it's not moving?
                                break;
                            case BOTTOM:
                                actionState.Land();
                                actionState.Jump();
                                score.IncreaseScore(100);
                                break;
                            case LEFT:
                                //Do Nothing. This will kick the shell, but won't affect Mario
                                Position = new Vector2(Position.X + 1, Position.Y);
                                break;
                            case RIGHT:
                                //Do Nothing. This will kick the shell, but won't affect Mario
                                Position = new Vector2(Position.X - 1, Position.Y);
                                break;
                        }
                    }
                }
            } else if (Collidee is Flag flag)
            {
                if (!(actionState is FlagState)) actionState = new FlagState(this, actionState.GetDirection());
                float slideHeight = ((flag.GetPosition().Y + flag.Sprite.texture.Height) - (GetPosition().Y + Sprite.texture.Height));
                System.Diagnostics.Debug.WriteLine("Distance from bottom: " + slideHeight);
                if (!WinningStateReached)
                {
                    int flagBonus = 0;
                    if (slideHeight <= 17)
                    {
                        flagBonus = 100;
                    }
                    else if (slideHeight <= 57)
                    {
                        flagBonus = 400;
                    }
                    else if (slideHeight <= 81)
                    {
                        flagBonus = 800;
                    }
                    else if (slideHeight <= 127)
                    {
                        flagBonus = 2000;
                    }
                    else if (slideHeight <= 153)
                    {
                        flagBonus = 4000;
                    }
                    else
                    {
                        IncrementLivesRemaining();
                    }
                    score.IncreaseScore(flagBonus);
                }
                WinningStateReached = true;
            }
        }
    

        private void CheckAndHandleIfAtScreenBoundary()
        {
            //This prevents Mario from going outside the screen
            if (newPosition.X > maxCoords.X)
            {
                this.SetXVelocity(0);
            }
            else if (newPosition.X < 0)
            {
                this.SetXVelocity(0);
            }

            if (newPosition.Y > maxCoords.Y)
            {
                if (!(powerState is DeadMario)) powerState = new DeadMario(this);
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
            if (actionState is FallingState || actionState is JumpingState)
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
            if (actionState is FallingState || actionState is JumpingState)
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

                if (Velocity.Y == 0 && pressType == 1)
                    actionState.Jump();
                else if (TimeJumpHeld > HighJumpTimer && Acceleration.Y >= FallingAcceleration - JumpHoldAccelerationBoost)
                    SetYAcceleration(Acceleration.Y - JumpHoldAccelerationBoost);
            }
            else if (pressType == 3)
            {
                TimeJumpHeld = 0;
                JumpIsHeld = false;
            }

            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
        }

        public void Down(int pressType)
        {
            if (!(powerState is DeadMario))
            {
                if (BlockMarioIsOn is WarpPipe pipe && pipe.CanWarp() && 
                    (actionState is IdleState || actionState is RunningState || actionState is CrouchingState) &&
                    GetPosition().X < 4000)
                {
                    // Warp to secret area!
                    hasWarped = true;
                    maxCoords = new Point(5000, maxCoords.Y);
                    SetPosition(pipe.GetWarpPosition());
                    actionState = new FallingState(this, actionState.GetDirection());
                } else if (BlockMarioIsOn is WarpPipe returnPipe && returnPipe.CanWarp() &&
                    (actionState is IdleState || actionState is RunningState || actionState is CrouchingState) &&
                    GetPosition().X >= 4000)
                {
                    hasWarped = false;
                    maxCoords = initialMaxCoords;
                    SetPosition(returnPipe.GetWarpPosition());
                    
                    actionState = new FallingState(this, actionState.GetDirection());
                    SetYVelocity(-120); // Launch out of pipe upwards!
                }
                else actionState.Crouch();
            }
            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
        }

        public override void Damage() 
        {
            this.powerState.TakeDamage();
        }

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
