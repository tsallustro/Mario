using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
using GameObjects;

namespace Commands
{
	class JumpCommand : MarioCommand
	{
		public JumpCommand(Mario mario)
			: base(mario)
		{
		}

		public override void Execute(int pressType)
		{
			// Call mario.Jump() here
			return;
		}
	}
}
