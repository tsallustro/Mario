using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprites;
using States;

namespace Factories
{
	public class GoombaSpriteFactory
	{
		private Texture2D goombaSprites;

		private static GoombaSpriteFactory factoryInstance = new GoombaSpriteFactory();

		private ISprite idleGoomba;
		private ISprite stompedGoomba;
		private ISprite movingGoomba;
		private ISprite deadGoomba;

		public static GoombaSpriteFactory Instance
		{
			get
			{
				return factoryInstance;
			}
		}

		private GoombaSpriteFactory()
		{
		}

		public void LoadTextures(Game game)
		{
			goombaSprites = game.Content.Load<Texture2D>("enemies");
		}

		/*
		 *  This method returns the correct sprite given the current action and
		 *  power-up states of Goomba.
		 */
		public ISprite GetCurrentSprite(Vector2 location, IEnemyState goombaState)
		{
			if (goombaState is IdleGoombaState)
			{
				return CreateIdleGoomba(location);
			}
			else if (goombaState is MovingGoombaState)
			{
				return CreateMovingGoomba(location);
			}
			else if (goombaState is StompedGoombaState)
			{
				return CreateStompedGoomba(location);
			}
			return CreateDeadGoomba(location);
		}
		public ISprite CreateIdleGoomba(Vector2 location)
		{
			if(idleGoomba == null)
            {
				idleGoomba = new Sprite(true, location, goombaSprites, 9, 15, 0, 0);
			    return idleGoomba;
			}
			else return idleGoomba;
		}
		public ISprite CreateMovingGoomba(Vector2 location)
		{
			if (movingGoomba == null)
			{
				movingGoomba = new Sprite(true, location, goombaSprites, 9, 15, 0, 1);
				return movingGoomba;
			}
			else return movingGoomba;
		}
		public ISprite CreateStompedGoomba(Vector2 location)
		{
			if (stompedGoomba == null)
			{
				stompedGoomba = new Sprite(true, location, goombaSprites, 9, 15, 2, 2);
				return stompedGoomba;
			}
			else return stompedGoomba;
		}
		public ISprite CreateDeadGoomba(Vector2 location)
		{
			if (deadGoomba == null)
			{
				deadGoomba = new Sprite(true, location, goombaSprites, 9, 15, 134, 134);
				return deadGoomba;
			}
			else return deadGoomba;
		}
	}
}
