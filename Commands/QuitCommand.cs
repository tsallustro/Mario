using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Sprites;
namespace Commands
{
    class QuitCommand : Command, ICommand
    {
        private Game game;

        public QuitCommand(Game game)
        {
            this.game = game;
        }

        public override void Execute(int pressType)
        {
            // 1:ExecuteOnPress
            if (pressType == 1 && active)
            {
                game.Exit();
            }
            
        }
    }
}
