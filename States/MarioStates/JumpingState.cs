﻿using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public class JumpingState : IMarioActionState
    {
        private Mario mario;
        private bool left;

        public JumpingState(Mario mario, bool left)
        {
            this.mario = mario;
            this.left = left;

            if (this.mario.Acceleration.Y <= 0)
            {
                /*
                 * Adjust these values to adjust jump height. Acceleration should be GREATER THAN
                 * velocity in order for gravity to feel correct.
                 */
                this.mario.SetYVelocity(-150);
                this.mario.SetYAcceleration(155);
            }
        }

        public bool GetDirection()
        {
            return this.left;
        }
        public void MoveRight()
        {
            if (this.left)
            {
                mario.SetActionState(new JumpingState(mario, !this.left));
            }
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.SetActionState(new JumpingState(mario, !this.left));
            }
        }

        public void Crouch()
        {
            // Do nothing
        }

        public void Jump()
        {
            //Do nothing, already in Jumping State
        }

        public void Fall()
        {
            mario.SetActionState(new FallingState(mario, this.left));
        }

        public void Land()
        {
            mario.SetActionState(new IdleState(mario, this.left));
        }

        public void Idle()
        {
            mario.SetActionState(new IdleState(mario, this.left));
        }
    }
}
