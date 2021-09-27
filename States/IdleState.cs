using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public class IdleState : IMarioActionState
    {
        private Mario mario;
        private bool left;

        public IdleState(Mario mario, bool left)
        {
            this.mario = mario;
            this.mario.SetSprite(mario.GetSpriteFactory().CreateStandardIdleMario(mario.GetSpriteLocation()));
            this.left = left;
        }

        public void MoveLeft()
        {
            if (this.left)
            {
                mario.SetActionState(new RunningState(mario, this.left));
            }
            else
            {
                this.left = !this.left;
                mario.SetActionState(new IdleState(mario, this.left));
            }
        }

        public void MoveRight()
        {
            if (!this.left)
            {
                mario.SetActionState(new RunningState(mario, this.left));
            }
            else
            {
                this.left = !this.left;
                mario.SetActionState(new IdleState(mario, this.left));
            }
        }

        public void Crouch()
        {
            mario.SetActionState(new CrouchingState(mario, this.left));
        }

        public void Jump()
        {
            mario.SetActionState(new JumpingState(mario, this.left));
        }
    }
}
