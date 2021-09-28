using System;
using System.Collections.Generic;
using System.Text;
using States;
using GameObjects;

namespace Commands
{
    class SuperMarioCommand : MarioCommand
    {
		public SuperMarioCommand(IAvatar avatar)
			: base(avatar)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				avatar.SetPowerState(new SuperMario(avatar));
			}
		}
	}
}
