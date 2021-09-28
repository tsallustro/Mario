using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
    class DeadMarioCommand : MarioCommand
    {
		public DeadMarioCommand(IAvatar avatar)
			: base(avatar)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				avatar.GetPowerState().TakeDamage();
			}
		}
	}
}
