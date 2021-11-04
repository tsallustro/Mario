using System;
using Sprites;
using GameObjects;

namespace Commands
{
	class MoveLeftCommand : MarioCommand
	{
		public MoveLeftCommand(IAvatar avatar) 
			: base(avatar)
		{
		}

		public override void Execute(int pressType)
		{
			if (active) avatar.MoveLeft(pressType);
		}
	}
}
