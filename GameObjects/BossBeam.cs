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
                if (instance == null)
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
        private Camera cam;

        public bool isActive { get; private set; } = false;

        public BossBeam() : base(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0))
        {
            this.Sprite = BossBeamSpriteFactory.Instance.CreateBeam(Position);
            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                       (Sprite.texture.Width / 6) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));

        }
        public void InitializeBeam(Mario mario, Camera camera)
        {
            this.mario = mario;
            this.cam = camera;
        }
        public override void Damage()
        {
            return; //The raw, god-like power of the beam precludes it from being damaged
        }
        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                base.Update(gameTime);
                Position += Velocity;
                Sprite.location += Velocity;
                Sprite.Update();
                if (cam.Limits is Rectangle rec && !rec.Contains(Position.X, Position.Y)) MakeInactive();
            }


        }
        public override void Collision(int side, GameObject Collidee)
        {
            if (isActive) {
                base.Collision(side, Collidee);
                if (Collidee is Bowser)
                {
                    ((Bowser)Collidee).Damage();
                    MakeInactive();
                }
                else if (Collidee is IEnemy)
                {
                    ((IEnemy)Collidee).Stomped();
                    MakeInactive();
                }
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
            this.Position = new Vector2(mario.GetPosition().X, mario.GetPosition().Y - 16);
            this.Sprite.location = this.Position;
            float XSpeed = BEAM_SPEED;
            if (left) XSpeed *= -1;
            this.SetXVelocity(XSpeed);
            System.Diagnostics.Debug.WriteLine("The Beam is now active.");
        }

        private void MakeInactive()
        {
            isActive = false;
            Halt();
            System.Diagnostics.Debug.WriteLine("The Beam is now inactive.");

        }
    }
}
