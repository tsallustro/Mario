using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
    class DeadMarioCommand : MarioCommand
    {
		public DeadMarioCommand(Mario mario)
			: base(mario)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				mario.SetPowerState(new DeadMario(mario));
			}
		}
	}
}
