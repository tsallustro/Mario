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
        private List<IGameObject> initialObjects;
        private MarioGame game;

        public LevelResetCommand(List<IGameObject> initialObjects, MarioGame game)
        {
            this.initialObjects = initialObjects;
            this.game = game;
        }

        public void Execute(int pressType)
        {
            // 1:ExecuteOnPress
            if (pressType == 1)
            {
                //objects.Clear();
                game.SetObjects(initialObjects);
            }

        }
    }
}
