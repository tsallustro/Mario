using System;
using System.Collections.Generic;
using System.Text;
using Sprites;

namespace CornetGame.GameObjects
{
    class Block
    {
        ISprite sprite;
        //IBlockState state;

        public Block(ISprite sprite)
        {
            this.sprite = sprite;
            //this.state = state;
        }
    }
}
