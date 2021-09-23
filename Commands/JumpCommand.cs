using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
	class JumpCommand : AvatarCommand
	{
		public JumpCommand(AvatarSprite sprite)
			: base(sprite)
		{
		}

		public override void Execute()
		{
			// Call sprite.Jump() here
			return;
		}
	}
}
