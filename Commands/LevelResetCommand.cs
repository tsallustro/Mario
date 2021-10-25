using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Game1;

namespace Commands
{
    class LevelResetCommand : ICommand
    {
        // We need a MarioGame here so we can call ResetObjects()
        private MarioGame game;

        public LevelResetCommand(MarioGame game)
        {
            this.game = game;
        }

        public void Execute(int pressType)
        {
            // 1:ExecuteOnPress
            if (pressType == 1)
            {
                //objects.Clear();
                game.ResetObjects();
            }

        }
    }
}
