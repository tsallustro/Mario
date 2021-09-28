using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public class IdleRedKoopaTroopaState : IEnemyState
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
    public class MovingRedKoopaTroopaState : IEnemyState
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

    public class StompedRedKoopaTroopaState : IEnemyState
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
    public class DeadRedKoopaTroopaState : IEnemyState
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
