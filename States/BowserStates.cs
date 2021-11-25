using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using Sound;

namespace States
{
    public class IdleBowserState : IEnemyState
    {
        private Bowser bowser;

        public IdleBowserState(Bowser bowser)
        {
            this.bowser = bowser;
        }

        public void Stomped()
        {
        }
        public void Move()
        {
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
    public class MovingBowserState : IEnemyState
    {
        private Bowser bowser;

        public MovingBowserState(Bowser bowser)
        {
            this.bowser = bowser;
        }

        public void Stomped()
        {

        }
        public void Move()
        {
            //Do Nothing. Goomba is already moving.
        }
        public void StayIdle()
        {

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

    public class DamagedBowserState : IEnemyState
    {
        private Bowser bowser;

        public DamagedBowserState(Bowser bowser)
        {
            this.bowser = bowser;
            
        }

        public void Stomped()
        {
            //Do nothing.
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
    public class DeadBowserState : IEnemyState
    {
        private Bowser bowser;

        public DeadBowserState(Bowser bowser)
        {
            this.bowser = bowser;
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
