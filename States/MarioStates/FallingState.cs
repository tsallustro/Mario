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
        private int InitialFallingAcceleration { get; } = 275; // Must be consistent across files
        private int MaxRunningAcceleration { get; } = 120;
        private float AccelerationIncrement { get; } = 15;
        private int OppositeDirectionMultiplier { get; } = 5;


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
            if (left)
            {
                left = !left;
            }

            if (mario.Acceleration.X < MaxRunningAcceleration) mario.SetXAcceleration(mario.Acceleration.X + AccelerationIncrement);
            else if (mario.Acceleration.X < 0) mario.SetXAcceleration(mario.Acceleration.X + (OppositeDirectionMultiplier * AccelerationIncrement));
        }

        public void MoveLeft()
        {
            if (!left)
            {
                left = !left;
            }

            if (mario.Acceleration.X > -MaxRunningAcceleration) mario.SetXAcceleration(mario.Acceleration.X - AccelerationIncrement);
            else if (mario.Acceleration.X > 0) mario.SetXAcceleration(mario.Acceleration.X - (OppositeDirectionMultiplier * AccelerationIncrement));
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
            mario.SetYVelocity(0);

            if (mario.ContinueRunning && initialLeft == left)
            {
                // If we changed direction while jumping, reset velocity to 0
                if ((!left && mario.GetVelocity().X < 0) || (left && mario.GetVelocity().X > 0))
                {
                    mario.SetXVelocity(0);
                }

                mario.SetActionState(new RunningState(mario, left));
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
