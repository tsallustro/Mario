using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
namespace Commands
{
	class JumpCommand : AvatarCommand
	{
		public JumpCommand(ISprite sprite)
			: base(sprite)
		{
		}

		public override void Execute(int pressType)
		{
			// Call sprite.Jump() here
			return;
		}
	}
}
