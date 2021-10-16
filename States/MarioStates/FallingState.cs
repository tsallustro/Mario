using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public class FallingState : IMarioActionState
    {
        private Mario mario;
        private bool left;
        private IMarioActionState previousState;

        public FallingState(Mario mario, bool left, IMarioActionState previousState)
        {
            this.mario = mario;
            this.left = left;
            this.previousState = previousState;

            mario.SetXAcceleration(0);

            if (mario.GetVelocity().Y < 0)
            {
                mario.SetYVelocity(-mario.GetVelocity().Y / 2);
            } else
            {
                mario.SetYAcceleration(155);
            }
        }

        public bool GetDirection()
        {
            return this.left;
        }

        public void MoveRight()
        {
            if (this.left)
            {
                mario.SetActionState(new FallingState(mario, !this.left, this.previousState));
            }
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.SetActionState(new FallingState(mario, !this.left, this.previousState));
            }
        }

        public void Crouch()
        {
            // Do nothing
        }

        public void Jump()
        {
            // Do nothing
        }

        public void Fall()
        {
            // Do nothing
        }

        public void Land()
        {
            float priorVel = mario.GetVelocity().X;
            mario.SetYVelocity(0);

            System.Diagnostics.Debug.WriteLine("Previous state: " + previousState);

            if (mario.Acceleration.X < 150 && mario.Acceleration.X > -150) 
                mario.SetActionState(new IdleState(mario, this.left));
            else if (previousState is RunningState)
            {
                mario.SetActionState(previousState);
                mario.SetXVelocity(priorVel);
            }

            
        }

        public void Idle()
        {
            mario.SetActionState(new IdleState(mario, this.left));
        }
    }
}
