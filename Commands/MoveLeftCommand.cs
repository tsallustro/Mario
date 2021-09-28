﻿using System;
using Sprites;
using GameObjects;

namespace Commands
{
	class MoveLeftCommand : MarioCommand
	{
		public MoveLeftCommand(Mario mario)
			: base(mario)
		{
		}

		public override void Execute(int pressType)
		{
			if(pressType == 1)
            {
				mario.MoveLeft();
			}
			
		}
	}
}
