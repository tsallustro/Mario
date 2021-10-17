using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public class JumpingState : IMarioActionState
    {
        private Mario mario;
        private bool left;
        private IMarioActionState previousState;

        // Physics variables
        private int InitialJumpingVelocity { get; } = -150;
        private int InitialJumpingAcceleration { get; } = 155; // Must be consistent across files

        public JumpingState(Mario mario, bool left, IMarioActionState previousState)
        {
            this.mario = mario;
            this.left = left;
            this.previousState = previousState;

            this.mario.SetXAcceleration(0);

            if (this.mario.Acceleration.Y <= 0)
            {
                /*
                 * Adjust these values to adjust jump height. Acceleration should be GREATER THAN
                 * velocity in order for gravity to feel correct.
                 */
                this.mario.SetYVelocity(InitialJumpingVelocity);
                this.mario.SetYAcceleration(InitialJumpingAcceleration);
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
                mario.SetActionState(new JumpingState(mario, !this.left, this.previousState));
            }
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.SetActionState(new JumpingState(mario, !this.left, this.previousState));
            }
        }

        public void Crouch()
        {
            // Do nothing
        }

        public void Jump()
        {
            // Do nothing, already in Jumping State
        }

        public void Fall()
        {
            mario.SetActionState(new FallingState(mario, this.left));
        }

        public void Land()
        {
            if (previousState is IdleState) mario.SetActionState(new IdleState(mario, this.left));
            else if (previousState is RunningState) mario.SetActionState(new RunningState(mario, this.left));
        }

        public void Idle()
        {
            mario.SetActionState(new IdleState(mario, this.left));
        }
    }
}
