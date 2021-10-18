using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using States;

namespace GameObjects
{
    public interface IBlock : IGameObject
    {
        void Bump();
        Vector2 GetLocation();
        void SetLocation(Vector2 location);
        void SetFalling(Boolean Falling);
        Boolean GetFalling();
        void SetBumped(Boolean Bumped);
        Boolean GetBumped();
        void SetBlockState(IBlockState blockState);

        List<IItem> GetItems();
        IItem RemoveItem();
    }
}
