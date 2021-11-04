using System;
using System.Collections.Generic;
using System.Text;
using Sprites;
using GameObjects;
using Microsoft.Xna.Framework.Media;
using Game1;

namespace Commands
{
	class muteBackgroundMusicCommand : ICommand
	{
		private MarioGame game;
		public muteBackgroundMusicCommand(MarioGame game)
		{
			this.game = game;
		}

        public void Execute(int pressType)
		{
			if (pressType == 1)
			{
				//_isMuted = !_isMuted;
				game.MuteMusic();
				
            }
        }
	}
}
