using System;

namespace Game1
{
	class MoveRightCommand : AvatarCommand
	{
		public MoveRightCommand(AvatarSprite sprite)
			: base(sprite)
		{
		}

		public override void Execute()
		{
			// Call sprite.MoveRight() here
			return;
		}
	}
}
