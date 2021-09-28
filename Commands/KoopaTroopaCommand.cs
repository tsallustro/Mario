using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
    abstract class KoopaTroopaCommand : ICommand 
    {
		protected KoopaTroopa koopaTroopa;
		public KoopaTroopaCommand(KoopaTroopa koopaTroopa)
		{
			this.koopaTroopa = koopaTroopa;
		}

		public abstract void Execute(int pressType);
	}

	class IdleKoopaTroopaCommand : KoopaTroopaCommand
	{
		public IdleKoopaTroopaCommand(KoopaTroopa koopaTroopa)
			:base(koopaTroopa)
		{
		}

		public override void Execute(int pressType)
        {
			if (pressType == 1)
			{
				koopaTroopa.StayIdle();
			}
		}
	}
	class MovingKoopaTroopaCommand : KoopaTroopaCommand
	{
		public MovingKoopaTroopaCommand(KoopaTroopa koopaTroopa)
			: base(koopaTroopa)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				koopaTroopa.Move();
			}
		}
	}
	class StompedKoopaTroopaCommand : KoopaTroopaCommand
	{
		public StompedKoopaTroopaCommand(KoopaTroopa koopaTroopa)
			: base(koopaTroopa)
		{
		}

		public override void Execute(int pressType)
		{
			if(pressType == 1)
            {
				koopaTroopa.Stomped();
			}
			
		}
	}
}
