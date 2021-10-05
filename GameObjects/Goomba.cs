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
    public class Goomba : IEnemy
    {
        private ISprite sprite;
        private IEnemyState goombaState;
        private GoombaSpriteFactory spriteFactory;
        private Vector2 location;

        public Goomba(Vector2 position)
        {
            spriteFactory = GoombaSpriteFactory.Instance;
            this.location = position;
            sprite = spriteFactory.CreateIdleGoomba(position);
            goombaState = new IdleGoombaState(this);
        }
        public IEnemyState GetGoombaState()
        {
            return this.goombaState;
        }
        public void SetGoombaState(IEnemyState goombaState)
        {
            this.goombaState = goombaState;
        }

        //Update all of Goomba's members
        public void Update(GameTime gameTime)
        {
            sprite = spriteFactory.GetCurrentSprite(sprite.location, goombaState);
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
            goombaState.Stomped();
        }
        //Change Goomba state to moving mode
        public void Move()
        {
            goombaState.Move();
        }
        //Change Goomba state to idle mode
        public void StayIdle()
        {
            goombaState.StayIdle();
        }

    }
}
