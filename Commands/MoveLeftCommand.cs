using System;

namespace Game1
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
