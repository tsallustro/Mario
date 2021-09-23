using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
    interface IMarioState
    {
        void MoveLeft();
        void MoveRight();
        void Crouch();
        void Jump();
        void DashOrThrowFireball();
    }
}
