﻿using System;
using Sprites;
using GameObjects;

namespace Commands
{
	/*
	 *  Abstract class for commands that apply to GameObject avatars.
	 */
	abstract class GameObjectCommand : ICommand
	{
		protected IGameObject avatar;

		protected GameObjectCommand(IGameObject avatar)
        {
			/*
			 *  Cast IAvatar to Mario so we can do Mario things.
			 *  This is probably not the best approach, but it works
			 *  for now.
			 */
			this.avatar = (IGameObject) avatar;
        }

		public abstract void Execute(int pressType);
	}
}