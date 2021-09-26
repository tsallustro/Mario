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
        IMarioPowerState powerState;
        IMarioActionState actionState;

        public Mario(ISprite startingSprite)
        {
            sprite = startingSprite;
            //powerState = new MarioPower();
            //actionState = new IdleState(this, false);
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

        public void MoveLeft()
        {

        }

        public void MoveRight()
        {

        }

        public void Jump()
        {

        }

        public void Crouch()
        {

        }

        //This class will need more methods as the project grows and the needs/abilities of Mario change. -Tony
    }
}
