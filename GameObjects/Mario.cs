using Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;


namespace GameObjects
{
    public class Mario : IAvatar
    {
        private ISprite sprite;
        private IMarioPowerState powerState;
        private IMarioActionState actionState;
        private MarioSpriteFactory spriteFactory;
        private Vector2 velocity;
        private Vector2 location;
        private enum actionStates {Idle, Crouching, Jumping, Falling, Running };
        GraphicsDeviceManager Graphics { get; set; }

        public Mario(Vector2 position, GraphicsDeviceManager graphics)
        {
            spriteFactory = MarioSpriteFactory.Instance;
            this.location = position;
            sprite = spriteFactory.CreateStandardIdleMario(location);
            powerState = new StandardMario(this);
            actionState = new IdleState(this, false);
            velocity = new Vector2(0, 0);
            Graphics = graphics;
        }

        public void SetXVelocity(float x)
        {
            this.velocity.X = x;
        }

        public void SetYVelocity(float y)
        {
            this.velocity.Y = y;
        }

        public IMarioPowerState GetPowerState()
        {
            return this.powerState;
        }

        public void SetPowerState(IMarioPowerState powerState)
        {
            this.powerState = powerState;
        }

        public void SetActionState(IMarioActionState actionState)
        {
            this.actionState = actionState;
        }

        //Update all of Mario's members
        public void Update(GameTime GameTime)
        {
            /* UNCOMMENT FOR SPRINT2
            float timeElapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;
            // Velocity calculations and state changes depending on velocity
            if (this.actionState is JumpingState || this.actionState is FallingState)
            {
                velocity.Y -= 1;
                if (velocity.Y < 0 && this.actionState is JumpingState)
                {
                    this.SetActionState(new FallingState(this, actionState.GetDirection()));
                }
                if (sprite.location.Y > Graphics.PreferredBackBufferHeight-16)
                {
                    this.SetActionState(new IdleState(this, actionState.GetDirection()));
                    velocity.Y = 0;
                    location.Y = Graphics.PreferredBackBufferHeight - 20;
                }
            }
            else
            {
                velocity.Y = 0;
            }
            location = location - velocity * timeElapsed;
            */
            //This prevents Mario from going outside the screen
            if (this.location.X > Graphics.PreferredBackBufferWidth) // Need to change this value to screen size - character size.
            {
                this.location.X = Graphics.PreferredBackBufferWidth;
            } else if (this.location.X < 0)
            {
                this.location.X = 0;
            }
            if (this.location.Y > Graphics.PreferredBackBufferHeight)
            {
                this.location.Y = Graphics.PreferredBackBufferHeight;
            }
            else if (this.location.Y < 0)
            {
                this.location.Y = 0;
            }
            sprite = spriteFactory.GetCurrentSprite(location, actionState, powerState);
            sprite.Update();
        }

        //Draw Mario
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.location = location;
            sprite.Draw(spriteBatch, actionState.GetDirection());
        }
        
        public void MoveLeft(int pressType)
        {
            actionState.MoveLeft();
            if (this.actionState is RunningState && pressType == 2)
            {
                this.location.X -= 1;
            }
        }
        
        public void MoveRight(int pressType)
        {
            //Mario only moves when holding the key
            actionState.MoveRight();
            //sprite.Move();
            if (this.actionState is RunningState && pressType == 2)
            {
                this.location.X += 1;
            }
        }

        public void Up(int pressType)
        {
            actionState.Jump();
            if (this.actionState is JumpingState && pressType == 2)
            {
                this.location.Y -= 1;
            }
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
        }

        public void Down(int pressType)
        {
            actionState.Crouch();
            if (this.actionState is CrouchingState && pressType == 2)
            {
                this.location.Y += 1;
            }
            /* UNCOMMENT FOR SPRINT2
            if (this.actionState is JumpingState || this.actionState is FallingState)
            {
                this.SetActionState(new IdleState(this, actionState.GetDirection()));
                velocity.Y = 0;
            }
            */
        }


    }
}
