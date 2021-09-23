using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class MovingAnimatedCommand : SpriteCommand
    {
        public MovingAnimatedCommand(ISprite sprite) 
            : base(sprite)
        {
        }

        public override void Execute()
        {
            sprite.ToggleVisibility();
        }
    }
}
