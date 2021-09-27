using System;
using System.Collections.Generic;
using System.Text;

namespace States
{
    public interface IMarioActionState
    {
        public bool GetDirection();
        public void MoveLeft();
        public void MoveRight();
        public void Crouch();
        public void Jump();

        //void DashOrThrowFireball(); FUTURE USE
    }
}
