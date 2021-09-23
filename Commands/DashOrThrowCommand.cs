using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
	class DashOrThrowCommand : AvatarCommand
	{
		public DashOrThrowCommand(ISprite sprite)
			: base(sprite)
		{
		}

		public override void Execute()
		{
			// Call sprite.DashOrThrow() here
			return;
		}
	}
}
