using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class NonMovingNonAnimatedCommand : SpriteCommand
    {
        public NonMovingNonAnimatedCommand(ISprite sprite) 
            : base(sprite)
        {
        }

        public override void Execute()
        {
            sprite.ToggleVisibility();
        }
    }
}
