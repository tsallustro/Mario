using System;
using System.Collections.Generic;
using System.Text;

namespace States
{
    public interface IEnemyState
    {
        public void Stomped();
        public void Move();
        public void StayIdle();
    }
}
