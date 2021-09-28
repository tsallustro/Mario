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
    public class KoopaTroopa : IEnemy
    {
        private ISprite sprite;
        private IEnemyState koopaTroopaState;
        private KoopaTroopaSpriteFactory spriteFactory;
        private Vector2 velocity;
        private Vector2 location;

        public KoopaTroopa(Vector2 position)
        {
            spriteFactory = KoopaTroopaSpriteFactory.Instance;
            this.location = position;
            sprite = spriteFactory.CreateIdleKoopaTroopa(position);
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
        public void Update()
        {
            sprite = spriteFactory.GetCurrentSprite(sprite.location, koopaTroopaState);
            sprite.Update();
        }

        //Draw Goomba
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, true);
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
