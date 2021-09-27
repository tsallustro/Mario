using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;

namespace GameObjects
{
    public class Block
    {
        ISprite sprite;
        IBlockState state;

        public Block(ISprite sprite, IBlockState state)
        {
            this.sprite = sprite;
            this.state = state;
        }

        public void SetSprite(ISprite sprite)
        {
            this.sprite = sprite;
        }

        public void SetBlockState(IBlockState state)
        {
            this.state = state;
        }

        public void Update()
        {
            sprite.Update();
        }

        public void Draw(SpriteBatch spriteBatch, bool direction)
        {
            sprite.Draw(spriteBatch, direction);
        }
    }
}
