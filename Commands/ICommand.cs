using System;
using Sprites;
namespace Commands
{
    public interface ICommand
    {
        // x corresponds to whether it is a keyboard press (1), hold (2), or release (3).
        void Execute(int pressType);
    }
}
