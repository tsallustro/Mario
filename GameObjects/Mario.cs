﻿using Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;


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
        GraphicsDeviceManager Graphics { get; set; }

        public Mario(Vector2 position, Vector2 velocity, Vector2 acceleration, GraphicsDeviceManager graphics, Point maxCoords)
            : base(position, velocity, acceleration)
        {
            spriteFactory = MarioSpriteFactory.Instance;
            Sprite = spriteFactory.CreateStandardIdleMario(position);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2), 
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            
            powerState = new StandardMario(this);
            actionState = new IdleState(this, false);
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
            newPosition = Position - Velocity * timeElapsed;
            //Position -= Velocity * timeElapsed;
            
            //This prevents Mario from going outside the screen
            if (newPosition.X > maxCoords.X)
            {
                newPosition = new Vector2(maxCoords.X, Position.Y);
            } else if (newPosition.X < 0)
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

            Position = newPosition;

            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            Sprite.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.location = Position;
            Sprite.Draw(spriteBatch, actionState.GetDirection());

            // Prepare AABB visualization
            if (BorderIsVisible)
            {
                int lineWeight = 2;
                Color lineColor = Color.Yellow;
                Texture2D boundary = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                boundary.SetData(new[] { Color.White });

                /* Draw rectangle for the AABB visualization */
                /* TODO - Only draw AABB visualization if the proper key has been pressed [Cc] */
                spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X, (int)AABB.Location.Y + lineWeight, lineWeight, AABB.Height - 2 * lineWeight), lineColor);               // left
                spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X, (int)AABB.Location.Y, AABB.Width - lineWeight, lineWeight), lineColor);                               // top
                spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X + AABB.Width - lineWeight, (int)AABB.Location.Y, lineWeight, AABB.Height - lineWeight), lineColor);    // right
                spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X, (int)AABB.Location.Y + AABB.Height - lineWeight, AABB.Width, lineWeight), lineColor);                 // bottom
            }
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
