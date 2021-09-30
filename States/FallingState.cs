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

        public FallingState(Mario mario, bool left)
        {
            this.mario = mario;
            this.left = left;
        }

        public bool GetDirection()
        {
            return this.left;
        }

        public void MoveRight()
        {
            if (this.left)
            {
                mario.SetActionState(new FallingState(mario, !this.left));
            }
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.SetActionState(new FallingState(mario, !this.left));
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
        public void Idle()
        {
            mario.SetActionState(new IdleState(mario, this.left));
        }
    }
}
