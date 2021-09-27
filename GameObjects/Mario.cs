using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using States;
using Sprites;
namespace GameObjects
{
    public class Mario
    {
        private ISprite sprite;
        private IMarioPowerState powerState;
        private IMarioActionState actionState;

        Mario(ISprite startingSprite)
        {
            sprite = startingSprite;
            powerState = new StandardMario(this);
            actionState = new IdleState(this, false);
        }

        public void SetPowerState(IMarioPowerState powerState)
        {
            this.powerState = powerState;
        }

        public void SetActionState(IMarioActionState actionState)
        {
            this.actionState = actionState;
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
            this.actionState.MoveLeft();
        }

        public void MoveRight()
        {
            this.actionState.MoveRight();
        }

        public void Jump()
        {
            this.actionState.Jump();
        }

        public void Crouch()
        {
            this.actionState.Crouch();
        }

        //This class will need more methods as the project grows and the needs/abilities of Mario change. -Tony
    }
}
