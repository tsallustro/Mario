using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public interface IBlockState
    {
        public void Bump(Mario mario);
    }
}
