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
            redKoopaTroopa.SetXVelocity((float)-50);
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
        public void Kicked(float sspeed)
        {
            //Do Nothing

        }
    }
    public class StompedRedKoopaTroopaState : IEnemyState
    {
        private RedKoopaTroopa redKoopaTroopa;
        private float previousVelocity;

        public StompedRedKoopaTroopaState(RedKoopaTroopa redKoopaTroopa)
        {
            this.redKoopaTroopa = redKoopaTroopa;
            previousVelocity = redKoopaTroopa.GetVelocity().X;
            this.redKoopaTroopa.SetXVelocity(0);
        }

        public void Stomped()
        {
            //redKoopaTroopa.SetRedKoopaTroopaState(new DeadRedKoopaTroopaState(redKoopaTroopa));
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
            redKoopaTroopa.SetRedKoopaTroopaState(new MovingRedKoopaTroopaState(redKoopaTroopa));
            redKoopaTroopa.SetXVelocity(previousVelocity);
        }
        public void Kicked(float sspeed)
        {
            redKoopaTroopa.SetXVelocity(sspeed);
            redKoopaTroopa.SetRedKoopaTroopaState(new MovingRedShelledKoopaTroopaState(redKoopaTroopa));
        }
    }
    public class MovingRedShelledKoopaTroopaState : IEnemyState
    {
        private RedKoopaTroopa redKoopaTroopa;

        public MovingRedShelledKoopaTroopaState(RedKoopaTroopa redKoopaTroopa)
        {
            this.redKoopaTroopa = redKoopaTroopa;
        }

        public void Stomped()
        {
            redKoopaTroopa.SetRedKoopaTroopaState(new StompedRedKoopaTroopaState(redKoopaTroopa));
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
        public void Kicked(float sspeed)
        {
            //Do Nothing
        }
    }


}

