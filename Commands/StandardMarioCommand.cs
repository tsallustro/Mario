using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
    class StandardMarioCommand : MarioCommand 
    {

		public StandardMarioCommand(IAvatar avatar)
			: base(avatar)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1 && active)
			{
				avatar.SetPowerState(new StandardMario(avatar));
			}
		}
	}
}
