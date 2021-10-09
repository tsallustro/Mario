using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public class RunningState2 : IMarioActionState2
    {
        private Mario2 mario;
        private bool left;
        IMarioActionState2 CurrentActionState { get; set; }
        IMarioActionState2 PreviousActionState { get; set; }

        public RunningState2(Mario2 mario, bool left)
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
            mario.SetXVelocity((float)0);
            mario.SetYVelocity((float)0);
        }
        public void CrouchingTransition()
        {
            mario.SetActionState2(new CrouchingState2(mario, this.left));
            mario.SetXVelocity((float)0);
            mario.SetYVelocity((float)0);
        }
        public void WalkingTransition()
        {
            float speed = 4;
            if (this.left)
                speed *= -1;
            mario.SetXVelocity(speed);
        }
        public void RunningTransition()
        {
            float speed = 8;
            if (this.left)
                speed *= -1;
            mario.SetXVelocity(speed);
        }
        public void JumpingTransition()
        {
            mario.SetActionState2(new JumpingState2(mario, this.left));
        }
        public void FallingTransition()
        {
            mario.SetActionState2(new FallingState2(mario, this.left));
        }
        public void FaceLeftTransition()
        {
            //You have to be in idle state first to face left
        }
        public void FaceRightTransition()
        {
            //You have to be in idle state first to face Right
        }
        public void CrouchingDiscontinueTransition()
        {
            //Not in crouching state
        }
        public void FaceLeftDiscontinueTransition()
        {
            //Not sure we need this
        }
        public void WalkingDiscontinueTransition()
        {
            mario.SetActionState2(new IdleState2(mario, this.left));
            mario.SetXVelocity((float)0);
        }
        public void RunningDiscontinueTransition()
        {
            float speed = 4;
            if (this.left)
                speed *= -1;
            mario.SetXVelocity(speed);
        }
        public void JumpingDiscontinueTransition()
        {
            //Not in Jumping State
        }
    }
}
