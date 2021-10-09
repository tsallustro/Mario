using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public class FallingState2 : IMarioActionState2
    {
        private Mario2 mario;
        private bool left;
        IMarioActionState2 CurrentActionState { get; set; }
        IMarioActionState2 PreviousActionState { get; set; }

        public FallingState2(Mario2 mario, bool left)
        {
            this.mario = mario;
            CurrentActionState = this.CurrentActionState;
            this.left = left;

        }
        public void Enter(IMarioActionState2 previousActionState)
        {
            this.PreviousActionState = previousActionState;
        }
        public void Exit()
        {
            mario.SetActionState2(PreviousActionState);
        }
        public void StandingTransition()
        {
            mario.SetActionState2(new IdleState2(mario, this.left));
        }
        public void CrouchingTransition()
        {
            //Mario doesn't crouch while falling
        }
        public void WalkingTransition()
        {
            //Mario can change direction midair
            float speed;
            mario.SetActionState2(new FallingState2(mario, this.left));
            if (this.left)
            {
                speed = -4;
            } else
            {
                speed = 4;
            }
            mario.SetXVelocity((speed));
        }
        public void RunningTransition()
        {
            //Mario doesn't acclerate midair
        }
        public void JumpingTransition()
        {
            //Mario doesn't double jump
        }
        public void FallingTransition()
        {
            //Mario is already falling
        }
        public void FaceLeftTransition()
        {
            mario.SetActionState2(new FallingState2(mario, true));
        }
        public void FaceRightTransition()
        {
            mario.SetActionState2(new FallingState2(mario, false));
        }
        public void CrouchingDiscontinueTransition()
        {
            //Mario is not crouching
        }
        public void FaceLeftDiscontinueTransition()
        {
            //Not sure we need it
        }
        public void WalkingDiscontinueTransition()
        {
            //Mario x direction becomes 0 when not moving side
            mario.SetActionState2(new FallingState2(mario, this.left));
            mario.SetXVelocity((float)0);
        }
        public void RunningDiscontinueTransition()
        {
            //Mario doesn't acclerate midair
        }
        public void JumpingDiscontinueTransition()
        {
            //Mario is already falling
        }
    }
}
