using System;
using System.Collections.Generic;
using System.Text;
using States;
using GameObjects;

namespace Commands
{
    class SuperMarioCommand : MarioCommand
    {
		public SuperMarioCommand(Mario mario)
			: base(mario)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				mario.SetPowerState(new SuperMario(mario));
			}
		}
	}
}
