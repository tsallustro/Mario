using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameObjects
{
    public interface IGameObject
    {
        void Update();
        void Draw(SpriteBatch spriteBatch);
    }
}
