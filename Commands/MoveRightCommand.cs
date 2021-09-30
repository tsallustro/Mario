using System;
using Sprites;
using GameObjects;

namespace Commands
{
	class MoveRightCommand : MarioCommand
	{
		public MoveRightCommand(IAvatar avatar) 
			: base(avatar)
		{
		}

		public override void Execute(int pressType)
		{ 
			if(pressType == 1)
            {
				avatar.MoveRight();
			} else if (pressType == 2)
			{
				avatar.MoveRight();
			} else if (pressType == 3)
            {
				avatar.MoveLeft();
            }
		}
	}
}
