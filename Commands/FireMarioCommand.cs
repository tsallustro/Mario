using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
    class FireMarioCommand : MarioCommand
    {
		public FireMarioCommand(Mario mario)
			: base(mario)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				mario.SetPowerState(new FireMario(mario));
			}
		}
	}
}
