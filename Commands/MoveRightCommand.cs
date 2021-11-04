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
			if (active) avatar.MoveRight(pressType);
		}
	}
}
