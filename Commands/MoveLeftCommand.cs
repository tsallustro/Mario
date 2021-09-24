using System;
using Sprites;
namespace Commands
{
	class MoveLeftCommand : AvatarCommand
	{
		public MoveLeftCommand(ISprite sprite)
			: base(sprite)
		{
		}

		public override void Execute(int pressType)
		{
			// Call sprite.MoveLeft() here
			return;
		}
	}
}
