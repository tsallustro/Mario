﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
	class JumpCommand : AvatarCommand
	{
		public JumpCommand(ISprite sprite)
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