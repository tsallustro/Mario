using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
using GameObjects;

namespace Commands
{
	class JumpCommand : MarioCommand
	{
		public JumpCommand(IAvatar avatar)
			: base(avatar)
		{
		}

		public override void Execute(int pressType)
		{
			if(pressType == 1)
            {
				avatar.Up(pressType);
			}else if (pressType == 2)
			{
				avatar.Up(pressType);
			}
		}
	}
}
