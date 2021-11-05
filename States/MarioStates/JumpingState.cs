using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using Sound;

namespace States
{
    public class JumpingState : IMarioActionState
    {
        private Mario mario;
        private bool left;
        private IMarioActionState previousState;

        // Physics variables
        private int InitialJumpingVelocity { get; } = -200;
        private int InitialJumpingAcceleration { get; } = 275; // Must be consistent across files
        private int RunningAcceleration { get; } = 100;

        public JumpingState(Mario mario, bool left, IMarioActionState previousState)
        {
            this.mario = mario;
            this.left = left;
            this.previousState = previousState;

            this.mario.SetXAcceleration(0);

            if (this.mario.Acceleration.Y <= 0)
            {
                this.mario.SetYVelocity(InitialJumpingVelocity);
                this.mario.SetYAcceleration(InitialJumpingAcceleration);
            }
            SoundManager.GameSound jumpSound;
            if (mario.GetPowerState() is StandardMario)
                jumpSound = SoundManager.GameSound.STANDARD_JUMP;
            else
                jumpSound = SoundManager.GameSound.SUPER_JUMP;
            SoundManager.Instance.PlaySound(jumpSound);
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
            } else
            {
                mario.SetXAcceleration(RunningAcceleration);
            }
        }

        public void MoveLeft()
        {
            if (!left)
            {
                left = !left;
            } else {
                mario.SetXAcceleration(-RunningAcceleration);
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
            if (previousState is IdleState) mario.SetActionState(new IdleState(mario, left));
            else if (previousState is RunningState)
            {
                if (previousState.GetDirection() == left)
                {
                    mario.SetActionState(previousState);
                } else
                {
                    mario.SetActionState(new IdleState(mario, !left));
                }
            }
        }

        public void Idle()
        {
            mario.SetActionState(new IdleState(mario, left));
        }
    }
}
