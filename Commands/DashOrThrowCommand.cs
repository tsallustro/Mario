using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
using GameObjects;

namespace Commands
{
	class DashOrThrowCommand : MarioCommand
	{
		public DashOrThrowCommand(Mario mario)
			: base(mario)
		{
		}

		public override void Execute(int pressType)
		{
			// Call mario.DashOrThrow() here
		}
	}
}
