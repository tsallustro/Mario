using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
	class CrouchCommand : AvatarCommand
	{
		public CrouchCommand(ISprite sprite)
			: base(sprite)
		{
		}

		public override void Execute()
		{
			// Call sprite.Crouch() here
			return;
		}
	}
}
