using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using States;
using Sprites;
using Factories;

namespace GameObjects
{
    public class Mario
    {
        private ISprite sprite;
        private IMarioPowerState powerState;
        private IMarioActionState actionState;
        private MarioSpriteFactory spriteFactory;

        public Mario()
        {
            spriteFactory = MarioSpriteFactory.Instance;
            sprite = spriteFactory.CreateStandardIdleMario(new Vector2(50, 25));
            powerState = new StandardMario(this);
            actionState = new IdleState(this, false);
        }

        public ISprite GetSprite()
        {
            return this.sprite;
        }

        public void SetSprite(ISprite sprite)
        {
            this.sprite = sprite;
        }

        public void SetPowerState(IMarioPowerState powerState)
        {
            this.powerState = powerState;
        }

        public void SetActionState(IMarioActionState actionState)
        {
            this.actionState = actionState;
        }

        public Vector2 GetSpriteLocation()
        {
            return sprite.location;
        }

        public MarioSpriteFactory GetSpriteFactory()
        {
            return spriteFactory;
        }

        //Update all of Mario's members
        public void Update()
        {
            sprite.Update();
        }

        //Draw Mario
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, actionState.GetDirection());
        }

        public void MoveLeft()
        {
            actionState.MoveLeft();
        }

        public void MoveRight()
        {
            actionState.MoveRight();
        }

        public void Jump()
        {
            actionState.Jump();
        }

        public void Crouch()
        {
            actionState.Crouch();
        }

        //This class will need more methods as the project grows and the needs/abilities of Mario change. -Tony
    }
}
