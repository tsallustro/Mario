using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class MovingNonAnimatedCommand : SpriteCommand
    {
        public MovingNonAnimatedCommand(ISprite sprite) 
            : base(sprite)
        {
        }

        public override void Execute(int pressType)
        {
            if (pressType == 1)
            {
                sprite.ToggleVisibility();
            }
        }
    }
}
