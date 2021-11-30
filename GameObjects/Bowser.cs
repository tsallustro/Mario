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
        private readonly int boundaryAdjustment = 8;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */

        private GameObject BlockEnemyIsOn { get; set; }
        private readonly int numberOfSpritesOnSheet = 10;
        private readonly double deathTimer = 1.5; 
        private double timeStomped = 0;

        private IBossState bowserState;
        private BowserSpriteFactory spriteFactory;
        private bool introduced = false;
        Vector2 newPosition;
        List<IGameObject> objects;
        Camera camera;
        GraphicsDeviceManager Graphics;

        Vector2 relativePos;
        float startMovingTiming = 0;
        float interval = 20;
        int i = 0;

        public Bowser(Vector2 position, Vector2 velocity, Vector2 acceleration, Camera camera, GraphicsDeviceManager graphics)
            : base(position, velocity, acceleration)
        {
            Graphics = graphics;
            spriteFactory = BowserSpriteFactory.Instance;
            Sprite = spriteFactory.CreateIdleBowser(position);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            Position = position;
            this.camera = camera;
            relativePos = new Vector2(0, Position.Y - camera.Position.Y);
            bowserState = new IdleBowserState(this, true);
            
        }

        //Reset
        public override void ResetObject()
        {


        }
        public void SetPosition(Vector2 position)
        {
            this.Position = position;
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
            this.bowserState.TakeDamage();
        }

        //Handles Collision with other Objects
        public override void Collision(int side, GameObject Collidee)
        {
            const int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;

            //When hit by Mario attack get damaged.

        }

        //Update
        public override void Update(GameTime GameTime)
        {
            float timeElapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;
            base.Update(GameTime);
            UpdateRelativePos();
            SetMoveTiming(GameTime);
            MoveAlongWithCamera();
            newPosition = Position + Velocity * timeElapsed;
            Position = newPosition;
            Sprite = spriteFactory.GetCurrentSprite(Position, bowserState);
            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            Sprite.Update();
        }

        //Draw
        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.location = Position;
            Sprite.Draw(spriteBatch, bowserState.GetDirection());
            DrawAABBIfVisible(Color.Yellow, spriteBatch);
        }
        public void UpdateRelativePos()
        {
            //Bowser's relative position within viewport needs to be updated constantly.
            if (Velocity == new Vector2(0,0))
            {
                relativePos.Y = Position.Y - camera.Position.Y;
            }
            
        }

        //Bowser position is fixed relative to the position of camera
        public void MoveAlongWithCamera()
        {
            //this.SetPosition(new Vector2(Position.X, camera.Position.Y + relativePos.Y));
            Position -= relativePos;
        }
        
        //Bowser should be temporarily invincible when damaged.
        public void TempInvincible()
        {

        }
        public void SetMoveTiming(GameTime GameTime)
        {
            float gameTime = (float)GameTime.TotalGameTime.TotalSeconds;
            if (!(bowserState is DeadBowserState))
            {

                if (gameTime > startMovingTiming + (interval * 2) * i && gameTime <= (startMovingTiming + interval) + (interval * 2)*i)
                {
                    MoveLeft();
                }
                else if (gameTime > (startMovingTiming + interval) + (interval * 2) * i && gameTime <= (startMovingTiming + 2 * interval) + (interval * 2) * i)
                {
                    MoveRight();
                    //MoveDown();
                }


                if (gameTime - (startMovingTiming + (2 * interval * (i + 1))) > 0)
                {
                    i++;
                }
            }

        }
        //Move Bowser toward left until it reaches certain x position
        public void MoveLeft()
        {
            if (Position.X > 10)
            {
                SetXVelocity(-100);
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
            if (Position.X < 700)
            {
                SetXVelocity(100);
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
            if (relativePos.Y > 20)
            {
                SetYVelocity(100);
            }
            else
            {
                SetYVelocity(0);
            }
        }
        //Move bowser towards down until it reaches ceratin height relative to camera position
        public void MoveDown()
        {
            if (relativePos.Y < 300)
            {
                SetYVelocity(100);
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
            return this.bowserState is DeadBowserState;
        }


    }
}
