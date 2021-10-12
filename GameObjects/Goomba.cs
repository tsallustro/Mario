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
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
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

        public override void Halt()
        {
            
        }

        public override void Damage()
        {
            goombaState.Stomped();
        }
        // Overide
        public void Collision(int side, GameObject Collidee)
        {

            if (side == 1)          //Top
            {
                if (Collidee is Mario)
                {
                    goombaState.Stomped();
                } else
                {
                    //do nothing
                }
            }
            else if (side == 2)     //Bottom
            {
                if (Collidee is Block )//&& block is bumped
                {
                    goombaState.Stomped();
                }
            }
            else if (side == 3)     //Left
            {

            }
            else if (side == 4)     //Right
            {

            }

        }


        //Update all of Goomba's members
        public override void Update(GameTime GameTime)
        {
            
            foreach (GameObject obj in objects)
            {
                if (obj != this)
                {
                    //If Goomba is stomped
                    // Need to check that Mario is travelling downwards
                    if (obj is Mario && this.TopCollision(obj) && obj.GetVelocity().Y > 0)
                    {
                            this.Stomped();
                    }
                    else if (obj is Block && this.LeftCollision(obj) || this.RightCollision(obj))
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
