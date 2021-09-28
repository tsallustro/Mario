using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
    abstract class GoombaCommand : ICommand 
    {
		protected Goomba goomba;
		public GoombaCommand(Goomba goomba)
		{
			this.goomba = goomba;
		}

		public abstract void Execute(int pressType);
	}

	class IdleGoombaCommand : GoombaCommand
	{
		public IdleGoombaCommand(Goomba goomba)
			:base(goomba)
		{
		}

		public override void Execute(int pressType)
        {
			if (pressType == 1)
			{
				goomba.StayIdle();
			}
		}
	}
	class MovingGoombaCommand : GoombaCommand
	{
		public MovingGoombaCommand(Goomba goomba)
			: base(goomba)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				goomba.Move();
			}
		}
	}
	class StompedGoombaCommand : GoombaCommand
	{
		public StompedGoombaCommand(Goomba goomba)
			: base(goomba)
		{
		}

		public override void Execute(int pressType)
		{
			if(pressType == 1)
            {
				goomba.Stomped();
			}
			
		}
	}
}
