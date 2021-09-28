using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprites;
using States;

namespace Factories
{
	public class RedKoopaTroopaSpriteFactory
	{
		private Texture2D redKoopaTroopaSprites;

		private static RedKoopaTroopaSpriteFactory factoryInstance = new RedKoopaTroopaSpriteFactory();

		private ISprite idleRedKoopaTroopa;
		private ISprite stompedRedKoopaTroopa;
		private ISprite movingRedKoopaTroopa;
		private ISprite deadRedKoopaTroopa;

		public static RedKoopaTroopaSpriteFactory Instance
		{
			get
			{
				return factoryInstance;
			}
		}

		private RedKoopaTroopaSpriteFactory()
		{
		}

		public void LoadTextures(Game game)
		{
			redKoopaTroopaSprites = game.Content.Load<Texture2D>("enemies");
		}

		/*
		 *  This method returns the correct sprite given the current action and
		 *  power-up states of Goomba.
		 */
		public ISprite GetCurrentSprite(Vector2 location, IRedKoopaTroopaState redKoopaTroopaState)
		{
			if (redKoopaTroopaState is IdleRedKoopaTroopaState)
			{
				return CreateIdleRedKoopaTroopa(location);
			}
			else if (redKoopaTroopaState is MovingRedKoopaTroopaState)
			{
				return CreateMovingRedKoopaTroopa(location);
			}
			else if (redKoopaTroopaState is StompedRedKoopaTroopaState)
			{
				return CreateStompedRedKoopaTroopa(location);
			}
			return CreateDeadRedKoopaTroopa(location);
		}
		public ISprite CreateIdleRedKoopaTroopa(Vector2 location)
		{
			if(idleRedKoopaTroopa == null)
            {
				idleRedKoopaTroopa = new Sprite(true, location, redKoopaTroopaSprites, 9, 15, 22, 22);
			    return idleRedKoopaTroopa;
			}
			else return idleRedKoopaTroopa;
		}
		public ISprite CreateMovingRedKoopaTroopa(Vector2 location)
		{
			if (movingRedKoopaTroopa == null)
			{
				movingRedKoopaTroopa = new Sprite(true, location, redKoopaTroopaSprites, 9, 15, 22, 23);
				return movingRedKoopaTroopa;
			}
			else return movingRedKoopaTroopa;
		}
		public ISprite CreateStompedRedKoopaTroopa(Vector2 location)
		{
			if (stompedRedKoopaTroopa == null)
			{
				stompedRedKoopaTroopa = new Sprite(true, location, redKoopaTroopaSprites, 9, 15, 27, 27);
				return stompedRedKoopaTroopa;
			}
			else return stompedRedKoopaTroopa;
		}
		public ISprite CreateDeadRedKoopaTroopa(Vector2 location)
		{
			if (deadRedKoopaTroopa == null)
			{
				deadRedKoopaTroopa = new Sprite(true, location, redKoopaTroopaSprites, 9, 15, 134, 134);
				return deadRedKoopaTroopa;
			}
			else return deadRedKoopaTroopa;
		}
	}
}
