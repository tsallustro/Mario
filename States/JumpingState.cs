using System;
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

            this.mario.SetYVelocity(100);
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
            mario.SetActionState(new IdleState(mario, this.left));
        }

        public void Jump()
        {
            //Do nothing, already in Jumping State
        }
        public void Idle()
        {
            mario.SetActionState(new IdleState(mario, this.left));
        }
    }
}
