using System;
using Sprites;
namespace Commands
{
    public interface ICommand
    {
        void SetActive(bool active);
        bool GetActive();
        void Execute(int pressType);
    }
}
