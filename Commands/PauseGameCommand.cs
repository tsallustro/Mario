using System;
using System.Collections.Generic;
using System.Text;
using Game1;

namespace Commands
{
    class PauseGameCommand : Command, ICommand
    {
        private MarioGame game; public PauseGameCommand(MarioGame game)
        {
            this.game = game;
        }
        public override void Execute(int pressType)
        {
            if (pressType == 1 && active)
            {
                game.TogglePause();
            }
        }
	}
}
