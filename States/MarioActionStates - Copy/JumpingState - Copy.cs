using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameObjects;

namespace States
{
    public class JumpingState2 : IMarioActionState2
    {
        private Mario2 mario;
        private bool left;
        private Vector2 origin;
        IMarioActionState2 CurrentActionState { get; set; }
        IMarioActionState2 PreviousActionState { get; set; }

        public JumpingState2(Mario2 mario, bool left)
        {
            this.mario = mario;
            CurrentActionState = new JumpingState2(mario, this.left);
            this.left = left;
            origin = mario.Location;

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
            //Can't crouch while jumping. Making it simple for now.
        }
        public void WalkingTransition()
        {
            //Mario can move right or left midair
            float speed;
            if (this.left == true)
            {
                speed = 4;
            } else
            {
                speed = -4;
            }
            mario.SetXVelocity(speed);
        }
        public void RunningTransition()
        {
            //Mario does not acclerate midair
        }
        public void JumpingTransition()
        {
            //Already Jumping
        }
        public void FallingTransition()
        {
            mario.SetActionState2(new FallingState2(mario, this.left));
        }
        public void FaceLeftTransition()
        {
            //Mario can change direction midair
            mario.SetActionState2(new JumpingState2(mario, true));
        }
        public void FaceRightTransition()
        {
            mario.SetActionState2(new JumpingState2(mario, false));
        }
        public void CrouchingDiscontinueTransition()
        {
            //Mario is not crouching            
        }
        public void FaceLeftDiscontinueTransition()
        {
            //Not sure this is needed. Professor mentioned it
        }
        public void WalkingDiscontinueTransition()
        {
            mario.SetActionState2(new JumpingState2(mario, this.left));
            mario.SetXVelocity((float)0);
        }
        public void RunningDiscontinueTransition()
        {
            //Mario doesn't acclerate midair.
        }
        public void JumpingDiscontinueTransition()
        {
            mario.SetActionState2(new FallingState2(mario, this.left));
        }
        public void Update()
        {
            //Set it so Mario starts falling when it reaches certain point.
        }
    }
}
