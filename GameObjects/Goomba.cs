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
    public class Goomba
    {
        private ISprite sprite;
        private IGoombaState goombaState;
        private GoombaSpriteFactory spriteFactory;

        public Goomba()
        {
            spriteFactory = GoombaSpriteFactory.Instance;
            sprite = spriteFactory.CreateIdleGoomba(new Vector2(300, 100));
            goombaState = new IdleGoombaState(this);
        }
        public IGoombaState GetGoombaState()
        {
            return this.goombaState;
        }
        public void SetGoombaState(IGoombaState goombaState)
        {
            this.goombaState = goombaState;
        }

        //Update all of Goomba's members
        public void Update()
        {
            sprite = spriteFactory.GetCurrentSprite(sprite.location, goombaState);
            sprite.Update();
        }

        //Draw Goomba
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, true);
        }

        public void Stomped()
        {
            goombaState.Stomped();
        }
        public void Move()
        {
            goombaState.Move();
        }
        public void StayIdle()
        {
            goombaState.StayIdle();
        }

    }
}
