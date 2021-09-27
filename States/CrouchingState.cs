using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public class CrouchingState : IMarioActionState
    {
        private Mario mario;
        private bool left;

        public CrouchingState(Mario mario, bool left)
        {
            this.mario = mario;
            this.mario.SetSprite(mario.GetSpriteFactory().CreateStandardCrouchingMario(mario.GetSpriteLocation()));
            this.left = left;
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.SetActionState(new CrouchingState(mario, !this.left));
            }
        }

        public void MoveRight()
        {
            if (this.left)
            {
                mario.SetActionState(new CrouchingState(mario, !this.left));
            }
        }

        public void Crouch()
        {
            //Do nothing, Mario is already crouched
        }

        public void Jump()
        {
            mario.SetActionState(new IdleState(mario, this.left));
        }
    }
}
