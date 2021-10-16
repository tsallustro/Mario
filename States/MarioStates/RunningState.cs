﻿using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    class RunningState : IMarioActionState
    {
        private Mario mario;
        private bool left;

        public RunningState(Mario mario, bool left)
        {
            this.mario = mario;
            this.left = left;

            this.mario.SetYAcceleration(0);

            if (this.left)
            {
                this.mario.SetXAcceleration(-150);
            }
            else
            {
                this.mario.SetXAcceleration(150);
            }
            
        }

        public bool GetDirection()
        {
            return this.left;
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.SetActionState(new IdleState(mario, this.left));
            } 
        }

        public void MoveRight()
        {
            if (this.left)
            {
                mario.SetActionState(new IdleState(mario, this.left));
            }
        }

        public void Crouch()
        {
            mario.SetXVelocity(0);
            mario.SetActionState(new CrouchingState(mario, this.left));
        }

        public void Jump()
        {
            mario.SetActionState(new JumpingState(mario, this.left, this));
        }

        public void Fall()
        {
            mario.SetActionState(new FallingState(mario, this.left, this));
        }

        public void Land()
        {
            //Do Nothing
        }
        public void Idle()
        {
            mario.SetActionState(new IdleState(mario, this.left));
        }
    }
}
