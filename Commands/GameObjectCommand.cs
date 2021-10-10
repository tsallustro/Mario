using System;
using Sprites;
using GameObjects;

namespace Commands
{
	/*
	 *  Abstract class for commands that apply to GameObject avatars.
	 */
	abstract class GameObjectCommand : ICommand
	{
		protected GameObject entity;

		protected GameObjectCommand(IGameObject entity)
        {
			this.entity = (GameObject) entity;
        }

		public abstract void Execute(int pressType);
	}
}
