﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    abstract class SpriteCommand : ICommand
    {
        protected ISprite sprite;

        protected SpriteCommand(ISprite sprite)
        {
            // Cast to Sprite from ISprite so we can use Sprite methods in concrete command classes
            this.sprite = sprite;
        }

        public abstract void Execute();
    }
}
