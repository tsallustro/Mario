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

        // x corresponds to whether it is a keyboard press (1), hold (2), or release (3).
        public abstract void Execute(int x);
    }
}
