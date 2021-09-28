using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public interface IGoombaState
    {
        public void Stomped();
        public void Move();
        public void StayIdle();
    }

    public class IdleGoombaState : IGoombaState
    {
        private Goomba goomba;

        public IdleGoombaState(Goomba goomba)
        {
            this.goomba = goomba;
        }

        public void Stomped()
        {
            goomba.SetGoombaState(new StompedGoombaState(goomba));
        }
        public void Move()
        {
            goomba.SetGoombaState(new MovingGoombaState(goomba));
        }
        public void StayIdle()
        {
            //Do Nothing.
        }
    }
    public class MovingGoombaState : IGoombaState
    {
        private Goomba goomba;

        public MovingGoombaState(Goomba goomba)
        {
            this.goomba = goomba;
        }

        public void Stomped()
        {
            goomba.SetGoombaState(new StompedGoombaState(goomba));
        }
        public void Move()
        {
            //Do Nothing. Goomba is already moving.
        }
        public void StayIdle()
        {
            goomba.SetGoombaState(new IdleGoombaState(goomba));
        }
    }

    public class StompedGoombaState : IGoombaState
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
    }
    //Once dead, Goomba's gone
    public class DeadGoombaState : IGoombaState
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
    }
  
  
}
