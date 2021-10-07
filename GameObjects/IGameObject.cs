using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace GameObjects
{
    public interface IGameObject
    {
        Vector2 position { get; set; }
        Vector2 velocity { get; set; }
        Vector2 accelaration { get; set; }
        float speed { get; set; }
        ISprite sprite { get; set; }
        List <IGameObject> objects { get; set; }

        // Need Rectangle to recognize the boundary?
       
        void Update(GameTime GameTime);
        void Draw(SpriteBatch spriteBatch);

        //Methods
        
        // Collision methods?
        bool IsTouchingRight(ISprite sprite);
        bool IsTouchingLeft(ISprite sprite);
        bool IsTouchingTop(ISprite sprite);
        bool IsTouchingBottom(ISprite sprite);


        void TakeDamage();
    }
}
