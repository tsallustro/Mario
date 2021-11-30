using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public class CoinState : IItemState
    {
        private IItem item;

        public CoinState(IItem item)
        {
            this.item = item;
        }
    }
    public class SuperMushroomState : IItemState
    {
        private IItem item;

        public SuperMushroomState(IItem item)
        {
            this.item = item;
        }
    }
    public class OneUpMushroomState : IItemState
    {
        private IItem item;

        public OneUpMushroomState(IItem item)
        {
            this.item = item;
        }
    }
    public class FireFlowerState : IItemState
    {
        private IItem item;

        public FireFlowerState(IItem item)
        {
            this.item = item;
        }
    }
    public class BossPowerUpState : IItemState
    {
        private IItem item;

        public BossPowerUpState(IItem item)
        {
            this.item = item;
        }
    }
    public class StarState : IItemState
    {
        private IItem item;

        public StarState(IItem item)
        {
            this.item = item;
        }
    }


}
