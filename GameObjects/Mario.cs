using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using States;
using Sprites;
namespace GameObjects
{
    class Mario
    {
        ISprite sprite;
        IMarioActionState actionState;
        IMarioPowerState powerState;
        public Mario(ISprite startingSprite)
        {
            //Set starting sprite/states
        }
        //Update all of Mario's members
        public void Update()
        {
            sprite.Update();
        }

        //Draw Mario
        public void Draw(SpriteBatch SpriteBatch)
        {
            sprite.Draw(SpriteBatch);
        }

        //This class will need more methods as the project grows and the needs/abilities of Mario change -Tony
    }
}
