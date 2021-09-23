using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
    public interface IMarioActionState
    {
        void MoveLeft();
        void MoveRight();
        void Crouch();
        void Jump();
        //void DashOrThrowFireball();
    }

    public class MarioAction
    {
        public IMarioActionState state;

        public MarioAction(bool left)
        {
            state = new IdleState(this, left);
        }
    }

    public class IdleState : IMarioActionState
    {
        private MarioAction mario;
        private bool left;

        public IdleState(MarioAction mario, bool left)
        {
            this.mario = mario;
            this.left = left;
        }

        public void MoveLeft()
        {
            if (this.left)
            {
                mario.state = new RunningState(mario, this.left);
            } else
            {
                this.left = !this.left;
                mario.state = new IdleState(mario, this.left);
            }
        }

        public void MoveRight()
        {
            if (!this.left)
            {
                mario.state = new RunningState(mario, this.left);
            } else {
                this.left = !this.left;
                mario.state = new IdleState(mario, this.left);
            }
        }

        public void Crouch()
        {
            mario.state = new CrouchingState(mario, this.left);
        }

        public void Jump()
        {
            mario.state = new JumpingState(mario, this.left);
        }


    }

    public class CrouchingState : IMarioActionState
    {
        private MarioAction mario;
        private bool left;

        public CrouchingState(MarioAction mario, bool left)
        {
            this.mario = mario;
            this.left = left;
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.state = new CrouchingState(mario, !this.left);
            }
        }

        public void MoveRight()
        {
            if (this.left)
            {
                mario.state = new CrouchingState(mario, !this.left);
            }
        }

        public void Crouch()
        {
            //Do nothing, Mario is already crouched
        }

        public void Jump()
        {
            mario.state = new JumpingState(mario, this.left);
        }
    }

    public class JumpingState : IMarioActionState
    {
        private MarioAction mario;
        private bool left;

        public JumpingState(MarioAction mario, bool left)
        {
            this.mario = mario;
            this.left = left;
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.state = new JumpingState(mario, !this.left);
            }
        }

        public void MoveRight()
        {
            if (this.left)
            {
                mario.state = new JumpingState(mario, !this.left);
            }
        }

        public void Crouch()
        {
            mario.state = new IdleState(mario, this.left);
        }

        public void Jump()
        {
            //Do nothing, already in Jumping State
        }
    }

    public class FallingState : IMarioActionState
    {
        private MarioAction mario;
        private bool left;

        public FallingState(MarioAction mario, bool left)
        {
            this.mario = mario;
            this.left = left;
        }

        public void MoveRight()
        {
            if (this.left)
            {
                mario.state = new FallingState(mario, !this.left);
            }
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.state = new FallingState(mario, !this.left);
            }
        }

        public void Crouch()
        {
            mario.state = new CrouchingState(mario, this.left);
        }

        public void Jump()
        {
            mario.state = new JumpingState(mario, this.left);
        }
    }

    public class RunningState : IMarioActionState
    {
        private MarioAction mario;
        private bool left;

        public RunningState(MarioAction mario, bool left)
        {
            this.mario = mario;
            this.left = left;
        }

        public void MoveLeft()
        {
            if (!this.left)
            {
                mario.state = new IdleState(mario, this.left);
            }
        }

        public void MoveRight()
        {
            if (this.left)
            {
                mario.state = new IdleState(mario, this.left);
            }
        }

        public void Crouch()
        {
            mario.state = new CrouchingState(mario, this.left);
        }

        public void Jump()
        {
            mario.state = new JumpingState(mario, this.left);
        }
    }
}
