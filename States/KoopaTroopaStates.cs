using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    //Base KoopaTroopa State. Normally KoopaTroopa won't be Idle, but moving
    public class IdleKoopaTroopaState : IEnemyState
    {
        private KoopaTroopa koopaTroopa;

        public IdleKoopaTroopaState(KoopaTroopa koopaTroopa)
        {
            this.koopaTroopa = koopaTroopa;
        }

        public void Stomped()
        {
            koopaTroopa.SetKoopaTroopaState(new StompedKoopaTroopaState(koopaTroopa));
        }
        public void Move()
        {
            koopaTroopa.SetKoopaTroopaState(new MovingKoopaTroopaState(koopaTroopa));
        }
        public void StayIdle()
        {
            //Do Nothing.
        }
    }
    public class MovingKoopaTroopaState : IEnemyState
    {
        private KoopaTroopa koopaTroopa;

        public MovingKoopaTroopaState(KoopaTroopa koopaTroopa)
        {
            this.koopaTroopa = koopaTroopa;
        }

        public void Stomped()
        {
            koopaTroopa.SetKoopaTroopaState(new StompedKoopaTroopaState(koopaTroopa));
        }
        public void Move()
        {
            //Do Nothing. Goomba is already moving.
        }
        public void StayIdle()
        {
            koopaTroopa.SetKoopaTroopaState(new IdleKoopaTroopaState(koopaTroopa));
        }
    }

    public class StompedKoopaTroopaState : IEnemyState
    {
        private KoopaTroopa koopaTroopa;

        public StompedKoopaTroopaState(KoopaTroopa koopaTroopa)
        {
            this.koopaTroopa = koopaTroopa;
        }

        public void Stomped()
        {
            koopaTroopa.SetKoopaTroopaState(new DeadKoopaTroopaState(koopaTroopa));
        }
        public void Move()
        {
            //Do nothing. It can't move while Stomped.
        }
        public void StayIdle()
        {
            //Do Nothing.
        }
        public void Revive()
        {
            //Do Revive
        }
    }
    public class MovingShelledKoopaTroopaState : IEnemyState
    {
        private KoopaTroopa koopaTroopa;

        public MovingShelledKoopaTroopaState(KoopaTroopa koopaTroopa)
        {
            this.koopaTroopa = koopaTroopa;
        }

        public void Stomped()
        {
            koopaTroopa.SetKoopaTroopaState(new StompedKoopaTroopaState(koopaTroopa));
        }
        public void Move()
        {
            //Do nothing. It can't move while Stomped.
        }
        public void StayIdle()
        {
            //Do Nothing.
        }
        public void Revive()
        {
            //Do Revive
        }
    }
    public class DeadKoopaTroopaState : IEnemyState
    {
        private KoopaTroopa koopaTroopa;

        public DeadKoopaTroopaState(KoopaTroopa koopaTroopa)
        {
            this.koopaTroopa = koopaTroopa;
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
