using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public interface IGoombaState
    {
        public void Stomped();
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

            //Do some stuff on used
        }
    }

    public class DeadGoombaState : IGoombaState
    {
        private Goomba goomba;

        public DeadGoombaState(Goomba goomba)
        {
            this.goomba = goomba;
        }

        public void Stomped()
        {
            //Do nothing. It's already dead
        }
    }
  
  
}
