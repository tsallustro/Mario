using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
    class FireMarioCommand : MarioCommand
    {
		public FireMarioCommand(IAvatar avatar)
			: base(avatar)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				avatar.SetPowerState(new FireMario(avatar));
			}
		}
	}
}
