using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Game1;

namespace Commands
{
    class LevelResetCommand : Command, ICommand
    {
        // We need a MarioGame here so we can call ResetObjects()
        private MarioGame game;

        public LevelResetCommand(MarioGame game)
        {
            this.game = game;
        }

        public override void Execute(int pressType)
        {
            if (pressType == 1 && active)
            {
                game.ResetObjects();
            }

        }
    }
}
