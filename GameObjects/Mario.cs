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
                if (sprite.location.Y > Graphics.PreferredBackBufferHeight)
                {
                    velocity.Y = 0;
                    this.SetActionState(new IdleState(this, actionState.GetDirection()));
                    sprite.location = new Vector2(sprite.location.X, Graphics.PreferredBackBufferHeight);
                }


            }
            else
            {
                velocity.Y = 0;
            }
            sprite.location = sprite.location - velocity * timeElapsed;

            sprite = spriteFactory.GetCurrentSprite(sprite.location, actionState, powerState);
            sprite.Update();









            sprite.Update();
        }

        //Draw Mario
        public void Draw(SpriteBatch spriteBatch)
        {
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
            actionState.Jump();
        }

        public void Crouch()
        {
            actionState.Crouch();
        }


    }
}
