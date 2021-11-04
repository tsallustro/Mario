using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
using GameObjects;

namespace Commands
{
	class throwFireballCommand : Command, ICommand
	{
		private FireBall fireball;

        public throwFireballCommand(FireBall FireBall)
		{
			fireball = FireBall;
		}

        public override void Execute(int pressType)
		{
			if (pressType == 1 && active)
			{
				fireball.ThrowFireBall();
			}
		}
	}
}
