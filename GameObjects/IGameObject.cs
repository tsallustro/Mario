using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace GameObjects
{
    public interface IGameObject
    {

        // Need Rectangle to recognize the boundary?

        void Update(GameTime GameTime);
        void Draw(SpriteBatch spriteBatch);

        //Methods
        void Add(List<IGameObject> objs);
        void Animate();


        // Collision methods?
        bool RightCollision(GameObject obj);
        bool LeftCollision(GameObject obj);
        bool TopCollision(GameObject obj);
        bool BottomCollision(GameObject obj);
    }
}  


