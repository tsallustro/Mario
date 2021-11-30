using Cameras;
using CornetGame.Factories;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CornetGame.GameObjects
{
    class BossBeam : GameObject
    {
        private Boolean left;
        private Mario mario;
        public BossBeam(Boolean Left, Mario Mario) : base(new Vector2(0, 0), new Vector2(5, 0), new Vector2(0, 250))
        {
            this.Sprite = BossBeamSpriteFactory.Instance.CreateBeam(Mario.GetPosition());
            this.left = Left;
            this.mario = Mario;
        }

        public override void Damage()
        {
            return;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, left);
        }

        public override void Halt()
        {
            this.SetXVelocity(0);
            this.SetYVelocity(0);
        }

        public override void ResetObject()
        {
            return;
        }
    }
}
