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
    public class Piranha : GameObject, IEnemy
    {
        private readonly int boundaryAdjustment = 5;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */

        private readonly int numberOfSpritesOnSheet = 7;


        private IEnemyState piranhaState;
        private PiranhaSpriteFactory spriteFactory;

        public Piranha(Vector2 position, Vector2 velocity, Vector2 acceleration)
            : base(position, velocity, acceleration)
        {
            this.Position = new Vector2(position.X, position.Y+10);

            // Save initial Data
            resetState.pos = this.Position;
            resetState.vel = velocity;
            resetState.acc = acceleration;

            this.SetIsPiped(true);

            spriteFactory = PiranhaSpriteFactory.Instance;
            Sprite = spriteFactory.CreateIdlePiranha(position);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            piranhaState = new IdlePiranhaState(this);
        }
        // reset Piranha to default state using initial data
        public override void ResetObject()
        {
            this.Position = resetState.pos;
            this.SetIsPiped(false);

            Sprite = spriteFactory.CreateActivePiranha(resetState.pos);
            piranhaState = new ActivePiranhaState(this);

        }


        //Get Piranha State
        public IEnemyState GetPiranhaState()
        {
            return this.piranhaState;
        }

        //Set Piranha State
        public void SetPiranhaState(IEnemyState piranhaState)
        {
            this.piranhaState = piranhaState;
        }

        public override void Halt()
        {
            //Do Nothing
        }
        public override void Damage()
        {
            this.SetIsPiped(true);
            this.Position = resetState.pos;
            Sprite = spriteFactory.CreateIdlePiranha(resetState.pos);
            this.SetPiranhaState(new IdlePiranhaState(this));

            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, (Sprite.texture.Height) - boundaryAdjustment));
        }


        //Handles Collision with other Objects
        public override void Collision(int side, GameObject Collidee)
        {
            if (Collidee is Mario mario)
            {
                if (mario.GetPowerState() is StarState)
                {
                    this.Damage();
                }
            }
            else if (Collidee is FireBall fireball && fireball.getActive())
            {
                this.Damage();
            }
        }

        //Update all of Piranha's members
        public override void Update(GameTime GameTime)
        {
            if (this.IsPiped)   // Short if Piranha is piped
            {
                return;
            }

            //float timeElapsed = (float)GameTime.ElapsedGameTime.TotalSeconds;
            Sprite = spriteFactory.GetCurrentSprite(Position, piranhaState);
            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, (Sprite.texture.Height) - boundaryAdjustment));
            Sprite.Update();
        }

        //Draw Piranha
        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.location = Position;
            Sprite.Draw(spriteBatch, true);
            DrawAABBIfVisible(Color.Red, spriteBatch);
        }

        public void Stomped()
        {
            this.Damage();
        }

        public bool IsDead()
        {
            return !IsPiped;
        }

        public void Move()
        {
            // Do Nothing
        }

        public void StayIdle()
        {
            // Do Nothing
        }
    }
}
