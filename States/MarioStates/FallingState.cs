using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public class FallingState : IMarioActionState
    {
        private Mario mario;
        private bool left;
        private IMarioActionState previousState;

        public FallingState(Mario mario, bool left, IMarioActionState previousState)
        {
            this.mario = mario;
            this.left = left;
            this.previousState = previousState;

            if (mario.GetVelocity().Y < 0)
            {
                mario.SetYVelocity(-mario.GetVelocity().Y / 2);
            } else
            {
                mario.SetYAcceleration(155);
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
                mario.SetActionState(new FallingState(mario, !this.left, this.previousState));
            }
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.SetActionState(new FallingState(mario, !this.left, this.previousState));
            }
        }

        public void Crouch()
        {
            // Do nothing
        }

        public void Jump()
        {
            // Do nothing
        }

        public void Fall()
        {
            // Do nothing
        }

        public void Land()
        {
            if (previousState is IdleState) mario.SetActionState(new IdleState(mario, this.left));
            else if (previousState is RunningState) mario.SetActionState(new RunningState(mario, this.left));
        }

        public void Idle()
        {
            mario.SetActionState(new IdleState(mario, this.left));
        }
    }
}
