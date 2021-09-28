using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
    abstract class RedKoopaTroopaCommand : ICommand 
    {
		protected RedKoopaTroopa redKoopaTroopa;
		public RedKoopaTroopaCommand(RedKoopaTroopa redKoopaTroopa)
		{
			this.redKoopaTroopa = redKoopaTroopa;
		}

		public abstract void Execute(int pressType);
	}

	class IdleRedKoopaTroopaCommand : RedKoopaTroopaCommand
	{
		public IdleRedKoopaTroopaCommand(RedKoopaTroopa redKoopaTroopa)
			:base(redKoopaTroopa)
		{
		}

		public override void Execute(int pressType)
        {
			if (pressType == 1)
			{
				redKoopaTroopa.StayIdle();
			}
		}
	}
	class MovingRedKoopaTroopaCommand : RedKoopaTroopaCommand
	{
		public MovingRedKoopaTroopaCommand(RedKoopaTroopa redKoopaTroopa)
			: base(redKoopaTroopa)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				redKoopaTroopa.Move();
			}
		}
	}
	class StompedRedKoopaTroopaCommand : RedKoopaTroopaCommand
	{
		public StompedRedKoopaTroopaCommand(RedKoopaTroopa redKoopaTroopa)
			: base(redKoopaTroopa)
		{
		}

		public override void Execute(int pressType)
		{
			if(pressType == 1)
            {
				redKoopaTroopa.Stomped();
			}
			
		}
	}
}
