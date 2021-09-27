using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public interface IMarioActionState
    {
        public void MoveLeft();
        public void MoveRight();
        public void Crouch();
        public void Jump();
        //void DashOrThrowFireball();
    }

    public class IdleState : IMarioActionState
    {
        private Mario mario;
        private bool left;

        public IdleState(Mario mario, bool left)
        {
            this.mario = mario;
            this.left = left;
        }

        public void MoveLeft()
        {
            if (this.left)
            {
                mario.SetActionState(new RunningState(mario, this.left));
            } else
            {
                this.left = !this.left;
                mario.SetActionState(new IdleState(mario, this.left));
            }
        }

        public void MoveRight()
        {
            if (!this.left)
            {
                mario.SetActionState(new RunningState(mario, this.left));
            } else {
                this.left = !this.left;
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

    public class CrouchingState : IMarioActionState
    {
        private Mario mario;
        private bool left;

        public CrouchingState(Mario mario, bool left)
        {
            this.mario = mario;
            this.left = left;
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.SetActionState(new CrouchingState(mario, !this.left));
            }
        }

        public void MoveRight()
        {
            if (this.left)
            {
                mario.SetActionState(new CrouchingState(mario, !this.left));
            }
        }

        public void Crouch()
        {
            //Do nothing, Mario is already crouched
        }

        public void Jump()
        {
            mario.SetActionState(new JumpingState(mario, this.left));
        }
    }

    public class JumpingState : IMarioActionState
    {
        private Mario mario;
        private bool left;

        public JumpingState(Mario mario, bool left)
        {
            this.mario = mario;
            this.left = left;
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.SetActionState(new JumpingState(mario, !this.left));
            }
        }

        public void MoveRight()
        {
            if (this.left)
            {
                 mario.SetActionState(new JumpingState(mario, !this.left));
            }
        }

        public void Crouch()
        {
            mario.SetActionState(new IdleState(mario, this.left));
        }

        public void Jump()
        {
            //Do nothing, already in Jumping State
        }
    }

    public class FallingState : IMarioActionState
    {
        private Mario mario;
        private bool left;

        public FallingState(Mario mario, bool left)
        {
            this.mario = mario;
            this.left = left;
        }

        public void MoveRight()
        {
            if (this.left)
            {
                mario.SetActionState(new FallingState(mario, !this.left));
            }
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.SetActionState(new FallingState(mario, !this.left));
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

    public class RunningState : IMarioActionState
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
