using Cameras;
using CornetGame.Factories;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameObjects
{
    class BossBeam : GameObject
    {
        private static BossBeam instance;
        public static BossBeam Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new BossBeam();
                }
                return instance;
            }
        }
        private static readonly int BEAM_SPEED = 5;
        protected readonly static int boundaryAdjustment = 0;

        private Boolean left;
        private Mario mario;
        public bool isActive = false;
      
        public BossBeam() : base(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0))
        {
            this.Sprite = BossBeamSpriteFactory.Instance.CreateBeam(Position);
            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                       (Sprite.texture.Width / 6) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));

        }
        public void InitializeBeam(Mario mario)
        {
            this.mario = mario;
        }
        public override void Damage()
        {
            return;
        }
        public override void Update(GameTime gameTime)
        {
            if(isActive)
            {
                base.Update(gameTime);
                Position += Velocity;
                Sprite.location += Velocity;
                Sprite.Update();

               
                
            }

           
        }
        public override void Collision(int side, GameObject Collidee)
        {
            base.Collision(side, Collidee);
            if(Collidee is Bowser)
            {
                ((Bowser)Collidee).Damage();
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                Sprite.Draw(spriteBatch, left);
                DrawAABBIfVisible(Color.Red, spriteBatch);
            }
        }
        
        public override void Halt()
        {
            this.SetXVelocity(0);
            this.SetYVelocity(0);
        }

        public override void ResetObject()
        {
            isActive = true;
            this.left = mario.isFacingLeft();
            this.Position = new Vector2(mario.GetPosition().X, mario.GetPosition().Y-16);
            this.Sprite.location = this.Position;
            float XSpeed= BEAM_SPEED;
            if (left) XSpeed *= -1;
            this.SetXVelocity(XSpeed);
        }
    }
}
