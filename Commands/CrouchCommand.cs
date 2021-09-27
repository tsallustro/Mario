using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
using GameObjects;

namespace Commands
{
	class CrouchCommand : MarioCommand
	{
		public CrouchCommand(Mario mario)
			: base(mario)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				mario.Crouch();
			}
		}
	}
}
