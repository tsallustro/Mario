using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace Commands
{
    abstract class BlockCommand : ICommand 
    {
		protected Block block;
		public BlockCommand(Block block)
		{
			this.block = block;
		}

		public abstract void Execute(int pressType);
	}
}
