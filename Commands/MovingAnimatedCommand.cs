using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprites;
namespace Commands
{
    class MovingAnimatedCommand : SpriteCommand
    {
        public MovingAnimatedCommand(ISprite sprite) 
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
