using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint0
{
    abstract class SpriteCommand : ICommand
    {
        protected Sprite sprite;

        protected SpriteCommand(ISprite sprite)
        {
            // Cast to Sprite from ISprite so we can use Sprite methods in concrete command classes
            this.sprite = (Sprite)sprite;
        }

        public abstract void Execute();
    }
}
