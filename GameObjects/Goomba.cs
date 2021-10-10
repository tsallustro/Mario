using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using States;
using Sprites;
using Factories;

namespace GameObjects
{
    public class Goomba : GameObject, IEnemy
    {
        private readonly int boundaryAdjustment = 4;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */
        private readonly int numberOfSpritesOnSheet = 3;
        private IEnemyState goombaState;
        private GoombaSpriteFactory spriteFactory;
        Vector2 newPosition;
        List<IGameObject> objects;


        public Goomba(Vector2 position, Vector2 velocity, Vector2 acceleration, List<IGameObject> objs)
            : base(position, velocity, acceleration)
        {
            spriteFactory = GoombaSpriteFactory.Instance;
            Sprite = spriteFactory.CreateMovingGoomba(position);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / 15) - boundaryAdjustment, (Sprite.texture.Height /9) - boundaryAdjustment));
            goombaState = new MovingGoombaState(this);
            objects = objs;
            this.SetXVelocity((float)50);
        }

        public IEnemyState GetGoombaState()
        {
            return this.goombaState;
        }

        public void SetGoombaState(IEnemyState goombaState)
        {
            this.goombaState = goombaState;
        }


        //Update all of Goomba's members
        public override void Update(GameTime GameTime)
        {
            
            foreach (var obj in objects)
            {
                if (obj != this)
                {
                    //If Goomba is stomped
                    if (this.TopCollision(obj))
                    {
                        if (obj is Mario)
                        {
                            this.Stomped();
                        }
                    }
                    else if (this.LeftCollision(obj) || this.RightCollision(obj))
                    {
                        //If goomba hits the wall
                        this.ChangeDirection();
                    }
                }
                    
                    
            }

            ChangeDirection();
            float timeElapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;
            newPosition = Position + (Velocity * timeElapsed);
            Position = newPosition;

            Sprite = spriteFactory.GetCurrentSprite(Position, goombaState);
            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
    (Sprite.texture.Width / 15) - boundaryAdjustment, (Sprite.texture.Height / 9) - boundaryAdjustment));
            Sprite.Update();
        }

        //Draw Goomba
        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.location = Position;
            Sprite.Draw(spriteBatch, true);

            // Prepare AABB visualization
            if (BorderIsVisible)
            {
                int lineWeight = 2;
                Color lineColor = Color.Red;
                Texture2D boundary = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                boundary.SetData(new[] { Color.White });

                /* Draw rectangle for the AABB visualization */
                spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X, (int)AABB.Location.Y + lineWeight, lineWeight, AABB.Height - 2 * lineWeight), lineColor);               // left
                spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X, (int)AABB.Location.Y, AABB.Width - lineWeight, lineWeight), lineColor);                               // top
                spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X + AABB.Width - lineWeight, (int)AABB.Location.Y, lineWeight, AABB.Height - lineWeight), lineColor);  // right
                spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X, (int)AABB.Location.Y + AABB.Height - lineWeight, AABB.Width, lineWeight), lineColor);  // bottom
            }
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
