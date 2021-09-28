using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using States;

namespace GameObjects
{
    public interface IBlock
    {
        void Update();
        void Draw(SpriteBatch spriteBatch);
        void Bump();
        /*
         * SetBlockState() is probably not needed in this interface,
         * but is required to work for now.
         */
        void SetBlockState(IBlockState blockState);
    }
}
