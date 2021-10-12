using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace GameObjects
{
    public interface IGameObject
    {
        void Update(GameTime GameTime);
        void Draw(SpriteBatch spriteBatch);

        //Methods
        Rectangle GetAABB();

        void SetXVelocity(float x);
        void SetYVelocity(float y);

        // Collision methods?
        void Collision(int side, GameObject Collidee);
        bool RightCollision(IGameObject obj);
        bool LeftCollision(IGameObject obj);
        bool TopCollision(IGameObject obj);
        bool BottomCollision(IGameObject obj);
    }
}  


