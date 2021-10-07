using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
    class DamageObjectCommand : GameObjectCommand
    {
		public DamageObjectCommand(IAvatar avatar)
			: base(avatar)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				avatar.TakeDamage();
			}
		}
	}
}
