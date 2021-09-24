using System;

namespace Game1
{
	class MoveRightCommand : AvatarCommand
	{
		public MoveRightCommand(ISprite sprite)
			: base(sprite)
		{
		}

		public override void Execute(int pressType)
		{
			// Call sprite.MoveRight() here
			return;
		}
	}
}
