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
    public class RedKoopaTroopa : IEnemy
    {
        private ISprite sprite;
        private IRedKoopaTroopaState redKoopaTroopaState;
        private RedKoopaTroopaSpriteFactory spriteFactory;
        private Vector2 velocity;
        private Vector2 location;

        public RedKoopaTroopa(Vector2 position)
        {
            spriteFactory = RedKoopaTroopaSpriteFactory.Instance;
            this.location = position;
            sprite = spriteFactory.CreateIdleRedKoopaTroopa(position);
            redKoopaTroopaState = new IdleRedKoopaTroopaState(this);
        }
        public IRedKoopaTroopaState GetRedKoopaTroopaState()
        {
            return this.redKoopaTroopaState;
        }
        public void SetRedKoopaTroopaState(IRedKoopaTroopaState redKoopaTroopaState)
        {
            this.redKoopaTroopaState = redKoopaTroopaState;
        }

        //Update all of Goomba's members
        public void Update()
        {
            sprite = spriteFactory.GetCurrentSprite(sprite.location, redKoopaTroopaState);
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
            redKoopaTroopaState.Stomped();
        }
        //Change Goomba state to moving mode
        public void Move()
        {
            redKoopaTroopaState.Move();
        }
        //Change Goomba state to idle mode
        public void StayIdle()
        {
            redKoopaTroopaState.StayIdle();
        }

    }
}
