using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprites;
namespace Commands
{
    class NonMovingNonAnimatedCommand : SpriteCommand
    {
        public NonMovingNonAnimatedCommand(ISprite sprite) 
            : base(sprite)
        {
        }

        public override void Execute(int x)
        {
            // 1:ExecuteOnPress
            if (x == 1)
            {
                sprite.ToggleVisibility();
            }
        }

    }
}
