using System;
using Sprites;
using GameObjects;

namespace Commands
{
	/*
	 *  Abstract class for commands that apply to the Mario avatar.
	 */
	abstract class MarioCommand : ICommand
	{
		protected Mario avatar;

		protected MarioCommand(IAvatar avatar)
        {
			/*
			 *  Cast IAvatar to Mario so we can do Mario things.
			 *  This is probably not the best approach, but it works
			 *  for now.
			 */
			this.avatar = (Mario) avatar;
        }

		public abstract void Execute(int pressType);
	}
}
