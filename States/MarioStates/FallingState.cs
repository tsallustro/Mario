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
        private bool initialLeft; // Save so we only continue in the same direction

        // Physics variables
        private int InitialFallingAcceleration { get; } = 155; // Must be consistent across files

        public FallingState(Mario mario, bool left)
        {
            this.mario = mario;
            this.left = left;
            initialLeft = left;

            mario.SetXAcceleration(0);

            if (mario.GetVelocity().Y < 0)
            {
                mario.SetYVelocity(-mario.GetVelocity().Y / 2);
            } else
            {
                mario.SetYAcceleration(InitialFallingAcceleration);
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
                left = !left;
            }
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                left = !left;
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

            if (mario.ContinueRunning && initialLeft == left)
            {
                mario.SetActionState(new RunningState(mario, left));
                mario.SetXVelocity(priorVel);
            } else
            {
                mario.SetActionState(new IdleState(mario, this.left));
            }

            mario.ContinueRunning = false;
        }

        public void Idle()
        {
            mario.SetActionState(new IdleState(mario, this.left));
        }
    }
}
