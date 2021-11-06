using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    class FlagState : IMarioActionState
    {
        private Mario mario;
        private bool left;
        private readonly int gravityAcceleration = 275;

        public FlagState(Mario mario, bool left)
        {
            this.mario = mario;
            this.left = left;

            this.mario.SetXAcceleration(0);
            this.mario.SetYAcceleration(gravityAcceleration);
            if (this.mario.GetVelocity().Y > 40) this.mario.SetYVelocity(40);
            this.mario.SetXVelocity(0);
        }

        public bool GetDirection()
        {
            return this.left;
        }

        public void MoveLeft()
        {
            // Do nothing
        }

        public void MoveRight()
        {
            // Do nothing
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
            mario.SetYAcceleration(0);
            mario.SetYVelocity(0);
            // Move into castle
        }

        public void Idle()
        {
            // Do nothing
        }
    }
}
