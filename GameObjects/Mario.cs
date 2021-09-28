using Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;


namespace GameObjects
{
    public class Mario
    {
        private ISprite sprite;
        private IMarioPowerState powerState;
        private IMarioActionState actionState;
        private MarioSpriteFactory spriteFactory;
        private Vector2 velocity;

        public Mario()
        {
            spriteFactory = MarioSpriteFactory.Instance;
            sprite = spriteFactory.CreateStandardIdleMario(new Vector2(50, 225));
            powerState = new StandardMario(this);
            actionState = new IdleState(this, false);
            velocity = new Vector2(0, 0);
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
        public void Update(GameTime GameTime, GraphicsDeviceManager Graphics)
        {
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

            sprite = spriteFactory.GetCurrentSprite(location, actionState, powerState);
            System.Diagnostics.Debug.WriteLine("AS:" + actionState);
            sprite.Update();
        }

        //Draw Mario
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.location = location;
            sprite.Draw(spriteBatch, actionState.GetDirection());
        }

        public void MoveLeft()
        {
            actionState.MoveLeft();
        }

        public void MoveRight()
        {
            actionState.MoveRight();
        }

        public void Jump()
        {
            velocity.Y = 100;
            this.SetActionState(new IdleState(this, actionState.GetDirection()));
            actionState.Jump();
            if (this.actionState is CrouchingState)
            {
                this.SetActionState(new IdleState(this, actionState.GetDirection()));
                velocity.Y = 0;
            }
        }

        public void Crouch()
        {
            actionState.Crouch();
            if (this.actionState is JumpingState || this.actionState is FallingState)
            {
                this.SetActionState(new IdleState(this, actionState.GetDirection()));
                velocity.Y = 0;
            }
        }


    }
}
