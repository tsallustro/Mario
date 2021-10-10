using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace Commands
{
    class BorderVisibleCommand : ICommand
    {
        private List<IGameObject> objects;

        public BorderVisibleCommand(List<IGameObject> objects)
        {
            this.objects = objects;
        }

        public void Execute(int pressType)
        {
            if (pressType == 1)
            {
                foreach (GameObject entity in objects)
                {
                    GameObject current = (GameObject)entity;
                    current.BorderIsVisible = true;
                }   
            }
        }
    }
}
