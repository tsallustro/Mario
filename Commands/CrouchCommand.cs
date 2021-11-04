using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
using GameObjects;

namespace Commands
{
	class CrouchCommand : MarioCommand
	{
		public CrouchCommand(IAvatar avatar)
			: base(avatar)
		{
		}

		public override void Execute(int pressType)
		{

			if (pressType == 1 && active)				// On Key Press
			{
				avatar.Down(pressType);
			}
		}
	}
}
