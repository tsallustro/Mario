using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace Commands
{
	class BumpCommand : BlockCommand
	{
		protected Mario mario;
		public BumpCommand(IBlock block, Mario Mario)
			: base(block)
		{
			mario = Mario;
		}


		public BumpCommand(IBlock block)
			: base(block)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1 && active)
			{
				block.Bump();
			}
		}
	}
}
