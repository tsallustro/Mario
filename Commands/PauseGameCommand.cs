using System;
using System.Collections.Generic;
using System.Text;
using Game1;

namespace Commands
{
    class PauseGameCommand : ICommand
	{
		private MarioGame game;
		public PauseGameCommand(MarioGame game)
		{
			this.game = game;
		}

		public void Execute(int pressType)
		{
			if (pressType == 1)
			{
				game.TogglePause();
			}
		}
	}
}
