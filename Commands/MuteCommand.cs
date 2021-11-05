using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
using GameObjects;
using Microsoft.Xna.Framework.Media;
using Game1;
using Sound;

namespace Commands
{
	class MuteCommand : Command, ICommand
	{
		private MarioGame game;
		public MuteCommand(MarioGame game)
		{
			this.game = game;
		}

        public override void Execute(int pressType)
		{
			if (pressType == 1 && active)
			{
				SoundManager.Instance.ToggleMute();
            }
        }
	}
}
