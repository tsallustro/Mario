using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public interface IKoopaTroopaState
    {
        public void Stomped();
        public void Move();
        public void StayIdle();
    }

    public class IdleKoopaTroopaState : IKoopaTroopaState
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
    public class MovingKoopaTroopaState : IKoopaTroopaState
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

    public class StompedKoopaTroopaState : IKoopaTroopaState
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
    }
    public class DeadKoopaTroopaState : IKoopaTroopaState
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
