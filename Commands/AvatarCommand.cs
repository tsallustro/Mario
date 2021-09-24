using System;
using Sprites;
namespace Commands
{
	/*
	 *  Abstract class for commands that apply to the Mario avatar.
	 */
	abstract class AvatarCommand : ICommand
	{
		protected ISprite sprite;

		protected AvatarCommand(ISprite sprite)
        {
			this.sprite = sprite;
        }

		public abstract void Execute(int pressType);
	}
}
