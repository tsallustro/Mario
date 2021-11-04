using System;
using System.Collections.Generic;
using System.Text;

namespace Commands
{
    abstract class Command : ICommand
    {
        protected bool active;

        public Command() { }

        public void SetActive(bool active)
        {
            this.active = active;
        }

        public bool GetActive()
        {
            return active;
        }

        public abstract void Execute(int pressType);
    }
}
