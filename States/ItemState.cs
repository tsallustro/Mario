﻿using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public interface ItemState
    {

    }
    /*
     * Doesn't include anything since, there's no behavior with the item.
     * But Might add something in the future
     */
    public class CoinState : ItemState
    {
        private Item item;

        public CoinState(Item item)
        {
            this.item = item;
        }
    }
    public class SuperMushroomState : ItemState
    {
        private Item item;

        public SuperMushroomState(Item item)
        {
            this.item = item;
        }
    }
    public class OneUpMushroomState : ItemState
    {
        private Item item;

        public OneUpMushroomState(Item item)
        {
            this.item = item;
        }
    }
    public class FireFlowerState : ItemState
    {
        private Item item;

        public FireFlowerState(Item item)
        {
            this.item = item;
        }
    }
    public class StarState : ItemState
    {
        private Item item;

        public StarState(Item item)
        {
            this.item = item;
        }
    }


}
