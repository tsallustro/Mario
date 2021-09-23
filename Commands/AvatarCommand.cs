using System;

namespace Game1
{
	/*
	 *  Abstract class for commands that apply to the Mario avatar.
	 */
	abstract class AvatarCommand : ICommand
	{
		protected AvatarSprite sprite;

		protected AvatarCommand(ISprite sprite)
        {
			// Cast to AvatarSprite to get added functionality
			this.sprite = (AvatarSprite)sprite;
        }

		public abstract void Execute();
	}
}
