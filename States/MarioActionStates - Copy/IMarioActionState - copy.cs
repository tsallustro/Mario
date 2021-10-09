using System;
using System.Collections.Generic;
using System.Text;

namespace States
{
    public interface IMarioActionState2
    {

        public void Enter(IMarioActionState2 previousActionState);
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

        //void DashOrThrowFireball(); FUTURE USE
    }
}
