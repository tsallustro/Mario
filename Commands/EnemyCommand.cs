using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace Commands
{
    abstract class EnemyCommand : ICommand
    {
		protected IEnemy enemy;

		public EnemyCommand(IEnemy enemy)
		{
			this.enemy = enemy;
		}

		public abstract void Execute(int pressType);
	}
}
