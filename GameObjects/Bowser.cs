using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using States;
using Sprites;
using Factories;
using Cameras;
using Sound;

namespace GameObjects
{
    public class Bowser : GameObject, IBoss
    {
        private readonly int boundaryAdjustment = 4;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */

        private GameObject BlockEnemyIsOn { get; set; }
        private readonly int numberOfSpritesOnSheet = 3;
        private readonly double deathTimer = 1.5; // Timer for stomped Goomba disappearing
        private double timeStomped = 0;

        private IBossState bowserState;
        //private GoombaSpriteFactory spriteFactory;
        private bool introduced = false;
        Vector2 newPosition;
        List<IGameObject> objects;
        Camera camera;

        public Bowser(Vector2 position, Vector2 velocity, Vector2 acceleration, List<IGameObject> objs, Camera camera)
            : base(position, velocity, acceleration)
        {
            //Initial position is placed top right
            Position = position;
            this.camera = camera;
        }
        // reset goomba to default state using initial data
        public override void ResetObject()
        {


        }

        //Get Bowser State
        public IBossState GetBowserState()
        {
            return this.bowserState;
        }

        //Set Bowser State
        public void SetBowserState(IBossState bowserState)
        {
            this.bowserState = bowserState;
        }

        public override void Halt()
        {
            //Do Nothing
        }
        public override void Damage()
        {
            TempInvincible();
        }
        
        //Handle Collision with Block
        private void HandleBlockCollision(int side, Block block)
        {
          //Do nothing. Bowser doesn't collide with blocks.
        }

        //Handles Collision with other Objects
        public override void Collision(int side, GameObject Collidee)
        {
            const int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;

            //When hit by Mario attack get damaged.

        }

        //Update all of Goomba's members
        public override void Update(GameTime GameTime)
        {

        }

        //Draw Goomba
        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        //Bowser position is fixed relative to the position of camera
        public void MoveAlongWithCamera(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float yOffset = Position.Y - camera.Position.Y;
            float xOffset = Position.X - camera.Position.X;

            Position = new Vector2(Position.X + xOffset, Position.Y + yOffset);

        }
        
        //Bowser should be temporarily invincible when damaged.
        public void TempInvincible()
        {
            bowserState.Damage();
        }
        
        //Move Bowser toward left until it reaches certain x position
        public void MoveLeft()
        {
            if (Position.X > 10)
            {
                SetXVelocity(-20);
                //Change bowser state to Moving bowser
            } else
            {
                SetXVelocity(0);
                //Set sprite to look towards right
                bowserState.FaceRight();
            }
        }
        //Move Bowser toward right until it reaches certain x position
        public void MoveRight()
        {
            if (Position.X < 780)
            {
                SetXVelocity(20);
                //Change bowser state to Moving bowser
            }
            else
            {
                SetXVelocity(0);
                //Set sprite to look towards left
                bowserState.FaceLeft();
            }
        }
        //Move bowser towards up until it reaches ceratin height relative to camera position
        public void MoveUp()
        {
            if (Position.Y - camera.Position.Y > -200)
            {
                SetYVelocity(20);
            }
            else
            {
                SetYVelocity(0);
            }
        }
        //Move bowser towards down until it reaches ceratin height relative to camera position
        public void MoveDown()
        {
            if (Position.Y - camera.Position.Y < 200)
            {
                SetYVelocity(20);
            }
            else
            {
                SetYVelocity(0);
            }
        }

        public void Attack ()
        {
            //Shoot fire
        }

        public bool IsDead()
        {
            return true;
        }


    }
}
