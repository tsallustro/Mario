using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public interface IRedKoopaTroopaState
    {
        public void Stomped();
        public void Move();
        public void StayIdle();
    }

    public class IdleRedKoopaTroopaState : IRedKoopaTroopaState
    {
        private RedKoopaTroopa redKoopaTroopa;

        public IdleRedKoopaTroopaState(RedKoopaTroopa redKoopaTroopa)
        {
            this.redKoopaTroopa = redKoopaTroopa;
        }

        public void Stomped()
        {
            redKoopaTroopa.SetRedKoopaTroopaState(new StompedRedKoopaTroopaState(redKoopaTroopa));
        }
        public void Move()
        {
            redKoopaTroopa.SetRedKoopaTroopaState(new MovingRedKoopaTroopaState(redKoopaTroopa));
        }
        public void StayIdle()
        {
            //Do Nothing.
        }
    }
    public class MovingRedKoopaTroopaState : IRedKoopaTroopaState
    {
        private RedKoopaTroopa redKoopaTroopa;

        public MovingRedKoopaTroopaState(RedKoopaTroopa redKoopaTroopa)
        {
            this.redKoopaTroopa = redKoopaTroopa;
        }

        public void Stomped()
        {
            redKoopaTroopa.SetRedKoopaTroopaState(new StompedRedKoopaTroopaState(redKoopaTroopa));
        }
        public void Move()
        {
            //Do Nothing. Goomba is already moving.
        }
        public void StayIdle()
        {
            redKoopaTroopa.SetRedKoopaTroopaState(new IdleRedKoopaTroopaState(redKoopaTroopa));
        }
    }

    public class StompedRedKoopaTroopaState : IRedKoopaTroopaState
    {
        private RedKoopaTroopa redKoopaTroopa;

        public StompedRedKoopaTroopaState(RedKoopaTroopa redKoopaTroopa)
        {
            this.redKoopaTroopa = redKoopaTroopa;
        }

        public void Stomped()
        {
            redKoopaTroopa.SetRedKoopaTroopaState(new DeadRedKoopaTroopaState(redKoopaTroopa));
        }
        public void Move()
        {
            //Do nothing. It can't move while Stomped.
        }
        public void StayIdle()
        {
            //Do Nothing.
        }
    }
    public class DeadRedKoopaTroopaState : IRedKoopaTroopaState
    {
        private RedKoopaTroopa redKoopaTroopa;

        public DeadRedKoopaTroopaState(RedKoopaTroopa redKoopaTroopa)
        {
            this.redKoopaTroopa = redKoopaTroopa;
        }

        public void Stomped()
        {
            //Do nothing.
        }
        public void Move()
        {
            //Do nothing. It's dead.
        }
        public void StayIdle()
        {
            //Do Nothing.
        }
    }
  
  
}
