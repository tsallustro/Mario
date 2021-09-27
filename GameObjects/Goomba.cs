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
            sprite = spriteFactory.CreateIdleGoomba(new Vector2(50, 25));
            goombaState = new IdleGoombaState(this);
        }

        public ISprite GetSprite()
        {
            return this.sprite;
        }

        public void SetSprite(ISprite sprite)
        {
            this.sprite = sprite;
        }

        public void SetGoombaState(IGoombaState goombaState)
        {
            this.goombaState = goombaState;
        }

        public Vector2 GetSpriteLocation()
        {
            return sprite.location;
        }

        public GoombaSpriteFactory GetSpriteFactory()
        {
            return spriteFactory;
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

    }
}
