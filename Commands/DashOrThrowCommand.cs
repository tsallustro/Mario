using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
using GameObjects;

namespace Commands
{
	class DashOrThrowCommand : MarioCommand
	{
		public DashOrThrowCommand(IAvatar avatar)
			: base(avatar)
		{
		}

		public override void Execute(int pressType)
		{
		}
	}
}
