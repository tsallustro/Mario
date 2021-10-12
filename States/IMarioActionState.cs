using System;
using System.Collections.Generic;
using System.Text;

namespace States
{
    public interface IMarioActionState
    {
        
        public bool GetDirection();
        public void MoveLeft();
        public void MoveRight();
        public void Crouch();
        public void Jump();

        public void Fall();

        public void Land();

        public void Idle();
        /*
        public void Enter(IMarioActionState previousActionState);
        public void Exit();
        public void StandingTransition();
        public void CrouchingTransition();
        public void WalkingTransition();
        public void RunningTransition();
        public void JumpingTransition();
        public void FallingTransition();
        public void FaceLeftTransition();
        public void FaceRightTransition();
        public void CrouchingDiscontinueTransition();
        public void FaceLeftDiscontinueTransition();
        public void WalkingDiscontinueTransition();
        public void RunningDiscontinueTransition();
        public void JumpingDiscontinueTransition();
        */

        //void DashOrThrowFireball(); FUTURE USE
    }
}
