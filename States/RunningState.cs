﻿using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    class RunningState : IMarioActionState
    {
        private Mario mario;
        private bool left;

        public RunningState(Mario mario, bool left)
        {
            this.mario = mario;
            this.left = left;
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.SetActionState(new IdleState(mario, this.left));
            }
        }

        public void MoveRight()
        {
            if (this.left)
            {
                mario.SetActionState(new IdleState(mario, this.left));
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
    }
}