using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
	class IdleRedKoopaTroopaCommand : EnemyCommand
	{
		public IdleRedKoopaTroopaCommand(IEnemy enemy)
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
	class MovingRedKoopaTroopaCommand : EnemyCommand
	{
		public MovingRedKoopaTroopaCommand(IEnemy enemy) : 
			base(enemy) 
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
	class StompedRedKoopaTroopaCommand : EnemyCommand
    {
        public StompedRedKoopaTroopaCommand(IEnemy enemy) 
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
