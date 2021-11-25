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
    public class Bowser : GameObject, IEnemy
    {
        private readonly float gravityAcceleration = 275;
        private readonly int boundaryAdjustment = 4;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */

        private GameObject BlockEnemyIsOn { get; set; }
        private readonly int numberOfSpritesOnSheet = 3;
        private readonly double deathTimer = 1.5; // Timer for stomped Goomba disappearing
        private double timeStomped = 0;

        private IEnemyState bowserState;
        private GoombaSpriteFactory spriteFactory;
        private bool introduced = false;
        Vector2 newPosition;
        List<IGameObject> objects;
        Camera camera;

        public Bowser(Vector2 position, Vector2 velocity, Vector2 acceleration, List<IGameObject> objs, Camera camera)
            : base(position, velocity, acceleration)
        {

        }
        // reset goomba to default state using initial data
        public override void ResetObject()
        {


        }

        //Get Goomba State
        public IEnemyState GetBowserState()
        {
            return this.bowserState;
        }

        //Set Goomba State
        public void SetBowserState(IEnemyState goombaState)
        {
            //this.goombaState = goombaState;
        }

        public override void Halt()
        {
            //Do Nothing
        }
        public override void Damage()
        {
            
            
        }
        

        //Handle Collision with Block
        private void HandleBlockCollision(int side, Block block)
        {
          

        }

        //Handles Collision with other Objects
        public override void Collision(int side, GameObject Collidee)
        {
            const int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;

        }

        //Update all of Goomba's members
        public override void Update(GameTime GameTime)
        {

        }

        //Draw Goomba
        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        //Change Goomba state to stomped mode
        public void Stomped()
        {

        }

        //Change Goomba state to moving mode
        public void Move()
        {

        }

        //Change Goomba state to idle mode
        public void StayIdle()
        {

        }

        public bool IsDead()
        {

        }


    }
}
