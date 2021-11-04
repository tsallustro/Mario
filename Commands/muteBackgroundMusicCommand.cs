using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
using GameObjects;
using Microsoft.Xna.Framework.Media;
using Game1;

namespace Commands
{
	class muteBackgroundMusicCommand : Command, ICommand
	{
		private MarioGame game;
		public muteBackgroundMusicCommand(MarioGame game)
		{
			this.game = game;
		}

        public override void Execute(int pressType)
		{
			if (pressType == 1 && active)
			{
				game.MuteMusic();
            }
        }
	}
}
