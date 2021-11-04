using System;
using Sprites;
using GameObjects;

namespace Commands
{
	/*
	 *  Abstract class for commands that apply to GameObject avatars.
	 */
	abstract class GameObjectCommand : Command, ICommand
	{
		protected GameObject entity;

		protected GameObjectCommand(GameObject entity)
        {
			this.entity = (GameObject) entity;
        }
	}
}
