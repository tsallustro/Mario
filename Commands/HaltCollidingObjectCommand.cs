using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
    class HaltCollidingObjectCommand : GameObjectCommand
    {
		public HaltCollidingObjectCommand(IGameObject entity)
			: base(entity)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				entity.SetYVelocity(0);
				entity.SetXVelocity(0);
			}
		}
	}
}
