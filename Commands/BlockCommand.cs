using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace Commands
{
    abstract class BlockCommand : Command, ICommand
    {
		protected IBlock block;

		protected BlockCommand(IBlock block)
		{
			this.block = block;
		}
	}
}
