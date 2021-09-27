﻿using System;
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
            sprite = spriteFactory.CreateStandardIdleMario(new Vector2(50, 225));
            powerState = new StandardMario(this);
            actionState = new IdleState(this, false);
        }

        public IMarioPowerState GetPowerState()
        {
            return this.powerState;
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
            sprite = spriteFactory.GetCurrentSprite(sprite.location, actionState, powerState);
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
