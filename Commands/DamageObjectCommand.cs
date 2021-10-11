using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
    class DamageObjectCommand : GameObjectCommand
    {
		public DamageObjectCommand(GameObject entity)
			: base(entity)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				entity.Damage();
				//Not sure what this is
				//avatar.velocity;
			}
		}
	}
}
