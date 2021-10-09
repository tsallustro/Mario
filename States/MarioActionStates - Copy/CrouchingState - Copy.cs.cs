using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public class CrouchingState2 : IMarioActionState2
    {
        private Mario2 mario;
        private bool left;
        IMarioActionState2 CurrentActionState { get; set; }
        IMarioActionState2 PreviousActionState { get; set; }

        public CrouchingState2(Mario2 mario, bool left)
        {
            this.left = left;
            this.mario = mario;
            CurrentActionState = new CrouchingState2(mario, left);
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
            this.Exit();
            mario.SetActionState2(new IdleState2(mario, left));
            this.Enter(CurrentActionState);
        }
        public void CrouchingTransition()
        {
            //Do Nothing
        }
        public void WalkingTransition()
        {
            //Can't walk while crouched. Let's make it simple for now. We'll add things later if needed.
        }
        public void RunningTransition()
        {
            //Can't run while crouched
        }
        public void JumpingTransition()
        {
            //Can't jump while crouched
        }
        public void FallingTransition()
        {
            //Can't Fall while crouched
        }
        public void FaceLeftTransition()
        {
            mario.SetActionState2(new CrouchingState2(mario, true));
        }
        public void FaceRightTransition()
        {
            mario.SetActionState2(new CrouchingState2(mario, false));
        }
        public void CrouchingDiscontinueTransition()
        {
            //Exit when discontinuing crouching
            Exit();
        }
        public void FaceLeftDiscontinueTransition()
        {
            //Not sure if this is needed, but this was included in the professor's feedback
        }
        public void WalkingDiscontinueTransition()
        {
            //Mario is not walking while crouched
        }
        public void RunningDiscontinueTransition()
        {
            //Mario is not running while crouched
        }
        public void JumpingDiscontinueTransition()
        {
            //Mario is not jumping
        }
        public void Update()
        {
        }
    }
}
