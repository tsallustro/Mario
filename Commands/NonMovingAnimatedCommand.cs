using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint0
{
    class NonMovingAnimatedCommand : SpriteCommand
    {
        public NonMovingAnimatedCommand(ISprite sprite) 
            : base(sprite)
        {
        }

        public override void Execute()
        {
            sprite.ToggleVisibility();
        }
    }
}
