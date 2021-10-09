using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
    class DamageObjectCommand : GameObjectCommand
    {
		public DamageObjectCommand(IGameObject avatar)
			: base(avatar)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				//Not sure what this is
				//avatar.velocity;
			}
		}
	}
}
