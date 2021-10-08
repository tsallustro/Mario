using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
    class HaltCollidingObjectCommand : GameObjectCommand
    {
		public HaltCollidingObjectCommand(GameObject avatar)
			: base(avatar)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				avatar.SetYVelocity(0);
				avatar.SetXVelocity(0);
			}
		}
	}
}
