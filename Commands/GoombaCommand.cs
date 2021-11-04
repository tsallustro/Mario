using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using States;

namespace Commands
{
	class IdleGoombaCommand : EnemyCommand
	{
		public IdleGoombaCommand(IEnemy enemy)
			: base(enemy)
		{
		}

		public override void Execute(int pressType)
        {
			if (pressType == 1 && active)
			{
				enemy.StayIdle();
			}
		}
	}
	class MovingGoombaCommand : EnemyCommand
    {
        public MovingGoombaCommand(IEnemy enemy) 
			: base(enemy) 
		{
		}

        public override void Execute(int pressType)
        {
            if (pressType == 1 && active)
            {
                enemy.Move();
            }
        }
	}
	class StompedGoombaCommand : EnemyCommand
    {
        public StompedGoombaCommand(IEnemy enemy) : 
			base(enemy) 
		{
		}

        public override void Execute(int pressType)
        {
            if (pressType == 1 && active)
            {
                enemy.Stomped();
            }
        }
	}
}
