using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using States;

namespace GameObjects
{
    public interface IItem
    {
        void Update();
        void Draw(SpriteBatch spriteBatch);
        /*
         *  Probably don't need SetItemState() in this interface if
         *  we can rework the main game class.
         */
        void SetItemState(IItemState itemState);
    }
}
