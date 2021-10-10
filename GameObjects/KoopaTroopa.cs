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
    public class KoopaTroopa : GameObject, IEnemy
    {
        private IEnemyState koopaTroopaState;
        private KoopaTroopaSpriteFactory spriteFactory;

        public KoopaTroopa(Vector2 position)
            : base(position, new Vector2(0, 0), new Vector2(0, 0))
        {
            spriteFactory = KoopaTroopaSpriteFactory.Instance;
            Sprite = spriteFactory.CreateIdleKoopaTroopa(position);
            koopaTroopaState = new IdleKoopaTroopaState(this);
        }

        public IEnemyState GetKoopaTroopaState()
        {
            return this.koopaTroopaState;
        }
        public void SetKoopaTroopaState(IEnemyState koopaTroopaState)
        {
            this.koopaTroopaState = koopaTroopaState;
        }

        //Update all of Goomba's members
        public override void Update(GameTime gameTime)
        {
            Sprite = spriteFactory.GetCurrentSprite(Sprite.location, koopaTroopaState);
            Sprite.Update();
        }

        //Draw Goomba
        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, true);
        }

        //Change Goomba state to stomped mode
        public void Stomped()
        {
            koopaTroopaState.Stomped();
        }
        //Change Goomba state to moving mode
        public void Move()
        {
            koopaTroopaState.Move();
        }
        //Change Goomba state to idle mode
        public void StayIdle()
        {
            koopaTroopaState.StayIdle();
        }

    }
}
