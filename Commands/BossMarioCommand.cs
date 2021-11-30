using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
    class BossMarioCommand : MarioCommand
    {
		public BossMarioCommand(IAvatar avatar)
			: base(avatar)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1 && active)
			{
				avatar.SetPowerState(new BossMario(avatar));
			}
		}
	}
}
