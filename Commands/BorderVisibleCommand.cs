using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace Commands
{
    class BorderVisibleCommand : Command, ICommand
    {
        private List<IGameObject> objects;

        public BorderVisibleCommand(List<IGameObject> objects)
        {
            this.objects = objects;
        }

        public override void Execute(int pressType)
        {
            if (pressType == 1 && active)
            {
                foreach (IGameObject entity in objects)
                {
                    GameObject current = (GameObject)entity;
                    current.BorderIsVisible = !current.BorderIsVisible;
                }   
            }
        }
    }
}
