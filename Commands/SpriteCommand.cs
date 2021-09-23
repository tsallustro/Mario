using System;
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
            this.sprite = sprite;
        }

        public abstract void Execute();
    }
}
