using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using Sound;

namespace States
{
    public class IdlePiranhaState : IEnemyState
    {
        private Piranha piranha;

        public IdlePiranhaState(Piranha piranha)
        {
            this.piranha = piranha;
        }

        public void Stomped()
        {
            // Do nothing
        }
        public void Move()
        {
            // Do nothing
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
    public class ActivePiranhaState : IEnemyState
    {
        private Piranha piranha;

        public ActivePiranhaState(Piranha piranha)
        {
            this.piranha = piranha;
        }

        public void Stomped()
        {
            // Do nothing
        }
        public void Move()
        {
            // Do nothing
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
