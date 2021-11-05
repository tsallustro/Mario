using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using Sound;

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
            koopaTroopa.SetXVelocity((float)-50);
        }
        public void StayIdle()
        {
            //Do Nothing.
        }
        public void Kicked(float sspeed)
        {
            //Do Nothing
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
        public void Kicked(float sspeed)
        {
            //Do Nothing
        }
    }

    public class StompedKoopaTroopaState : IEnemyState
    {
        private KoopaTroopa koopaTroopa;
        private float previousVelocity;

        public StompedKoopaTroopaState(KoopaTroopa koopaTroopa)
        {
            this.koopaTroopa = koopaTroopa;
            previousVelocity = koopaTroopa.GetVelocity().X;
            this.koopaTroopa.SetXVelocity(0);
            SoundManager.Instance.PlaySound(SoundManager.GameSound.STOMP);
        }

        public void Stomped()
        {
            //koopaTroopa.SetKoopaTroopaState(new DeadKoopaTroopaState(koopaTroopa));
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
            koopaTroopa.SetKoopaTroopaState(new MovingKoopaTroopaState(koopaTroopa));
            koopaTroopa.SetXVelocity(previousVelocity);
        }
        public void Kicked(float sspeed)
        {
            koopaTroopa.SetXVelocity(sspeed);
            koopaTroopa.SetKoopaTroopaState(new MovingShelledKoopaTroopaState(koopaTroopa));
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
        public void Kicked(float sspeed)
        {
            //Do Nothing
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
        public void Kicked(float sspeed)
        {
        }
    }
  
  
}
