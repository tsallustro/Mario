using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game1
{
    class QuitCommand : ICommand
    {
        public Game game;

        public QuitCommand(Game game)
        {
            this.game = game;
        }

        public void Execute()
        {
            game.Exit();
        }
    }
}
