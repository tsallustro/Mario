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
			if(pressType == 1)
            {
				avatar.MoveLeft(pressType);
			}/* else if (pressType == 2)
            {
				avatar.MoveLeft(pressType);
            }
			*/
		}
	}
}
