using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
namespace Commands
{
	class DashOrThrowCommand : AvatarCommand
	{
		public DashOrThrowCommand(ISprite sprite)
			: base(sprite)
		{
		}

		public override void Execute(int pressType)
		{

			// Call sprite.DashOrThrow() here
			return;
		}
	}
}
