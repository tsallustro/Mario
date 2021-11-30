using System;
using System.Collections.Generic;
using System.Text;

namespace States
{
    public interface IBossState
    {
        public void FaceRight();
        public void FaceLeft();
        public void Damage();

    }
}
