using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace Commands
{
	class BumpCommand : BlockCommand
	{
		public BumpCommand(Block block)
			: base(block)
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
