using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

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

	class BlockBumpCommand : BlockCommand
	{
		public BlockBumpCommand(Block block)
			:base(block)
		{
		}

		public override void Execute(int pressType)
        {
			if (pressType == 1)
			{
				block.Bump();
			}
		}
	}

}
