﻿using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public class IdleState2 : IMarioActionState2
    {
        private Mario2 mario;
        private bool left;
        IMarioActionState2 CurrentActionState { get; set; }
        IMarioActionState2 PreviousActionState { get; set; }

        public IdleState2(Mario2 mario, bool left)
        {
            this.left = left;
            this.mario = mario;
            CurrentActionState = this.CurrentActionState;

        }
        public void Enter(IMarioActionState2 previousActionState)
        {
            this.PreviousActionState = previousActionState;
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
            Exit();
            mario.SetActionState2(new CrouchingState2(mario, left));
            Enter(CurrentActionState);
        }
        public void WalkingTransition()
        {
            this.Exit();
            mario.SetActionState2(new RunningState2(mario, left));
            mario.SetXVelocity((float)4);
            this.Enter(CurrentActionState);
        }
        public void RunningTransition()
        {
            this.Exit();
            mario.SetActionState2(new RunningState2(mario, left));
            mario.SetXVelocity((float)8);
            this.Enter(CurrentActionState);
        }
        public void JumpingTransition()
        {
            this.Exit();
            mario.SetActionState2(new JumpingState2(mario, left));
            this.Enter(CurrentActionState);
        }
        public void FallingTransition()
        {
            this.Exit();
            mario.SetActionState2(new FallingState2(mario, left));
            this.Enter(CurrentActionState);
        }
        public void FaceLeftTransition()
        {
            this.Exit();
            mario.SetActionState2(new IdleState2(mario, true));
            this.Enter(CurrentActionState);
        }
        public void FaceRightTransition()
        {
            this.Exit();
            mario.SetActionState2(new IdleState2(mario, false));
            this.Enter(CurrentActionState);
        }
        public void CrouchingDiscontinueTransition()
        {
            //No action, so no discontinue
        }
        public void FaceLeftDiscontinueTransition()
        {
            //Not sure if this is needed, but this was included in the professor's feedback
        }
        public void WalkingDiscontinueTransition()
        {
            //No action, so no discontinue
        }
        public void RunningDiscontinueTransition()
        {
            //No action, so no discontinue
        }
        public void JumpingDiscontinueTransition()
        {
            //No action, so no discontinue
        }
        public void Update()
        {
        }
    }
}