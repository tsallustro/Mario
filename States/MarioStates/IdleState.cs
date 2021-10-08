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
        IMarioActionState previousState { get; set; }

        public IdleState(Mario mario, bool left)
        {
            this.mario = mario;
            this.left = left;

            this.mario.SetXVelocity(0);
            this.mario.SetYVelocity(0);
        }
        /*
        public void Enter(IMarioActionState previousActionState)
        {
            this.previousState = previousActionState;
        }
        public void Exit()
        {
            //Do Nothing
        }
        public void StandingTransition()
        {
            //Do Nothing. Already in Idle State
        }
        public void CrouchingTransition()
        {
            mario.SetActionState(new CrouchingState(mario, this.left));
        }
        public void WalkingTransition()
        {

        }
        public void RunningTransition()
        {

        }
        public void JumpingTransition()
        {

        }
        public void FallingTransition()
        {

        }
        public void FaceLeftTransition()
        {

        }
        public void FaceRightTransition()
        {

        }
        public void CrouchingDiscontinueTransition()
        {

        }
        public void FaceLeftDiscontinueTransition()
        {

        }
        public void WalkingDiscontinueTransition()
        {

        }
        public void RunningDiscontinueTransition()
        {

        }
        public void JumpingDiscontinueTransition()
        {

        }
        */
        

        public bool GetDirection()
        {
            return this.left;
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
        public void Idle()
        {
            mario.SetActionState(new IdleState(mario, this.left));
        }
        
    }
}
