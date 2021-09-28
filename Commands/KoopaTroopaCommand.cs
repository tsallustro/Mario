using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
	class IdleKoopaTroopaCommand : EnemyCommand
	{
		public IdleKoopaTroopaCommand(IEnemy enemy)
			:base(enemy)
		{
		}

		public override void Execute(int pressType)
        {
			if (pressType == 1)
			{
				enemy.StayIdle();
			}
		}
	}
	class MovingKoopaTroopaCommand : EnemyCommand
	{
		public MovingKoopaTroopaCommand(IEnemy enemy)
			: base(enemy)
		{
		}

		public override void Execute(int pressType)
		{
			if (pressType == 1)
			{
				enemy.Move();
			}
		}
	}
	class StompedKoopaTroopaCommand : EnemyCommand
    {
        public StompedKoopaTroopaCommand(IEnemy enemy) 
			: base(enemy)
		{ 
		}

        public override void Execute(int pressType)
        {
            if (pressType == 1)
            {
                enemy.Stomped();
            }
        }
	}
}
