using Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;


namespace GameObjects
{
    public class Mario : GameObject, IAvatar
    {
        private IMarioPowerState powerState;
        private IMarioActionState actionState;
        private MarioSpriteFactory spriteFactory;
        GraphicsDeviceManager Graphics { get; set; }

        public Mario(Vector2 position, Vector2 velocity, Vector2 acceleration, GraphicsDeviceManager graphics)
            : base(position, velocity, acceleration)
        {
            // TODO - Since this is main character, probably have to set closer to 0
            int boundaryAdjustment = 4;

            spriteFactory = MarioSpriteFactory.Instance;
            Sprite = spriteFactory.CreateStandardIdleMario(position);
            /* 
             * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
             * on that sheet!
             */
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2), 
                (Sprite.texture.Width / 15) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            powerState = new StandardMario(this);
            actionState = new IdleState(this, false);
            Graphics = graphics;
        }

        public IMarioPowerState GetPowerState()
        {
            return this.powerState;
        }

        public void SetPowerState(IMarioPowerState powerState)
        {
            this.powerState = powerState;
            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
        }

        public void SetActionState(IMarioActionState actionState)
        {
            this.actionState = actionState;
        }

        public override void Animate() { }
        public override void TakeDamage() { }

        //Update all of Mario's members
        public override void Update(GameTime GameTime)
        {

            float timeElapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;
            Position = Position - Velocity * timeElapsed;
            
            //This prevents Mario from going outside the screen
            if (this.Position.X > Graphics.PreferredBackBufferWidth) // TODO: Need to change this value to screen size - character size.
            {
                Position = new Vector2(Graphics.PreferredBackBufferWidth, Position.Y);
            } else if (this.Position.X < 0)
            {
                Position = new Vector2(0, Position.Y);
            }
            if (this.Position.Y > Graphics.PreferredBackBufferHeight)
            {
                Position = new Vector2(Position.X, Graphics.PreferredBackBufferHeight);
            }
            else if (this.Position.Y < 0)
            {
                Position = new Vector2(Position.X, 0);
            }
            Sprite.Update();
        }

        //Draw Mario
        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.location = Position;
            Sprite.Draw(spriteBatch, actionState.GetDirection());

            // Prepare AABB visualization
            int lineWeight = 2;
            Color lineColor = Color.Yellow;
            Texture2D boundary = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            boundary.SetData(new[] { Color.White });

            /* Draw rectangle for the AABB visualization */
            /* TODO - The line weight screws up the display of the dimensions of the AABB, need to fix it */
            /* TODO - Only draw AABB visualization if the proper key has been pressed [Cc] */
            spriteBatch.Draw(boundary, new Rectangle((int)Position.X, (int)Position.Y, lineWeight, AABB.Height), lineColor);
            spriteBatch.Draw(boundary, new Rectangle((int)Position.X, (int)Position.Y, AABB.Width, lineWeight), lineColor);
            spriteBatch.Draw(boundary, new Rectangle((int)Position.X + AABB.Width, (int)Position.Y, lineWeight, AABB.Height), lineColor);
            spriteBatch.Draw(boundary, new Rectangle((int)Position.X, (int)Position.Y + AABB.Height, AABB.Width, lineWeight), lineColor);
        }
        
        public void MoveLeft(int pressType)
        {
            /*actionState.MoveLeft();
            if (this.actionState is RunningState && pressType == 2)
            {
                this.location.X -= 1;
            }*/

            actionState.MoveLeft();
            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
        }

        public void MoveRight(int pressType)
        {
            //Mario only moves when holding the key
            actionState.MoveRight();

            //sprite.Move();
            /*if (this.actionState is RunningState && pressType == 2)
            {
                this.location.X += 1;
            }*/
            Sprite = spriteFactory.GetCurrentSprite(Position, actionState, powerState);
        }

        public void Up(int pressType)
        {
            actionState.Jump();
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
            actionState.Crouch();
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


    }
}
