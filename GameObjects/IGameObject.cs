using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameObjects
{
    public interface IGameObject
    {
        void Update(GameTime GameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
