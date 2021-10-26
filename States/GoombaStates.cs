using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public class IdleGoombaState : IEnemyState
    {
        private Goomba goomba;

        public IdleGoombaState(Goomba goomba)
        {
            this.goomba = goomba;
        }

        public void Stomped()
        {
            goomba.SetGoombaState(new StompedGoombaState(goomba));
            goomba.SetXVelocity((float)0);
        }
        public void Move()
        {
            goomba.SetGoombaState(new MovingGoombaState(goomba));
            goomba.SetXVelocity((float)100);
        }
        public void StayIdle()
        {
            //Do Nothing.
        }
        public void Enter()
        {

        }
        public void Exit()
        {

        }
        public void Kicked(float sspeed)
        {
            //Do Nothing
        }
    }
    public class MovingGoombaState : IEnemyState
    {
        private Goomba goomba;

        public MovingGoombaState(Goomba goomba)
        {
            this.goomba = goomba;
            //goomba.SetXVelocity((float)100);
        }

        public void Stomped()
        {
            goomba.SetGoombaState(new StompedGoombaState(goomba));
            goomba.SetXVelocity((float)0);
        }
        public void Move()
        {
            //Do Nothing. Goomba is already moving.
        }
        public void StayIdle()
        {
            goomba.SetGoombaState(new IdleGoombaState(goomba));
            goomba.SetXVelocity((float)0);
        }

        public void Enter()
        {

        }
        public void Exit()
        {

        }
        public void Kicked(float sspeed)
        {
            //Do Nothing
        }
    }

    public class StompedGoombaState : IEnemyState
    {
        private Goomba goomba;

        public StompedGoombaState(Goomba goomba)
        {
            this.goomba = goomba;
        }

        public void Stomped()
        {
            goomba.SetGoombaState(new DeadGoombaState(goomba));
        }
        public void Move()
        {
            //Do nothing. It can't move while Stomped.
        }
        public void StayIdle()
        {
            //Do Nothing.
        }

        public void Enter()
        {

        }
        public void Exit()
        {

        }
        public void Kicked(float sspeed)
        {
            //Do Nothing
        }
    }
    //Once dead, Goomba's gone
    public class DeadGoombaState : IEnemyState
    {
        private Goomba goomba;

        public DeadGoombaState(Goomba goomba)
        {
            this.goomba = goomba;
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

        public void Enter()
        {

        }
        public void Exit()
        {

        }
        public void Kicked(float sspeed)
        {
            //Do Nothing
        }
    }
  
  
}
