using System;
using Sprites;
using GameObjects;

namespace Commands
{
	class MoveRightCommand : MarioCommand
	{
		public MoveRightCommand(Mario mario)
			: base(mario)
		{
		}

		public override void Execute(int pressType)
		{
			if(pressType == 1)
            {
				mario.MoveRight();
			}
		}
	}
}
