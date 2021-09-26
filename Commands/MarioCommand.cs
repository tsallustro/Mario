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
		protected Mario mario;

		protected MarioCommand(Mario mario)
        {
			this.mario = mario;
        }

		public abstract void Execute(int pressType);
	}
}
