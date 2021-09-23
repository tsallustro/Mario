using System;

namespace Game1
{
	class MoveLeftCommand : AvatarCommand
	{
		public MoveLeftCommand(AvatarSprite sprite)
			: base(sprite)
		{
		}

		public override void Execute()
		{
			// Call sprite.MoveLeft() here
			return;
		}
	}
}
