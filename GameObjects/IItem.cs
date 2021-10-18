using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using States;

namespace GameObjects
{
    public interface IItem : IGameObject
    {
        void SetItemState(IItemState itemState);
        IItemState GetItemState();
        bool GetVisibility();
        void SetVisibility(bool isVisible);
        void MakeVisibleAndEmerge();
    }
}
