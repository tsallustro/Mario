using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprites;
using States;

namespace Factories
{
	public class KoopaTroopaSpriteFactory
	{
		private Texture2D koopaTroopaSprites;

		private static KoopaTroopaSpriteFactory factoryInstance = new KoopaTroopaSpriteFactory();

		private ISprite idleKoopaTroopa;
		private ISprite stompedKoopaTroopa;
		private ISprite movingKoopaTroopa;
		private ISprite deadKoopaTroopa;

		public static KoopaTroopaSpriteFactory Instance
		{
			get
			{
				return factoryInstance;
			}
		}

		private KoopaTroopaSpriteFactory()
		{
		}

		public void LoadTextures(Game game)
		{
			koopaTroopaSprites = game.Content.Load<Texture2D>("KoopaTroopa");
		}

		/*
		 *  This method returns the correct sprite given the current action and
		 *  power-up states of Goomba.
		 */
		public ISprite GetCurrentSprite(Vector2 location, IEnemyState koopaTroopaState)
		{
			if (koopaTroopaState is IdleKoopaTroopaState)
			{
				return CreateIdleKoopaTroopa(location);
			}
			else if (koopaTroopaState is MovingKoopaTroopaState)
			{
				return CreateMovingKoopaTroopa(location);
			}
			else if (koopaTroopaState is StompedKoopaTroopaState)
			{
				return CreateStompedKoopaTroopa(location);
			}
			return CreateDeadKoopaTroopa(location);
		}
		public ISprite CreateIdleKoopaTroopa(Vector2 location)
		{
			if(idleKoopaTroopa == null)
            {
				idleKoopaTroopa = new Sprite(false, true, location, koopaTroopaSprites, 1, 3, 0, 0);
			    return idleKoopaTroopa;
			}
			else return idleKoopaTroopa;
		}
		public ISprite CreateMovingKoopaTroopa(Vector2 location)
		{
			if (movingKoopaTroopa == null)
			{
				movingKoopaTroopa = new Sprite(false, true, location, koopaTroopaSprites, 1, 3, 0, 1);
				return movingKoopaTroopa;
			}
			else return movingKoopaTroopa;
		}
		public ISprite CreateStompedKoopaTroopa(Vector2 location)
		{
			if (stompedKoopaTroopa == null)
			{
				stompedKoopaTroopa = new Sprite(false, true, location, koopaTroopaSprites, 1, 3, 2, 2);
				return stompedKoopaTroopa;
			}
			else return stompedKoopaTroopa;
		}
		public ISprite CreateDeadKoopaTroopa(Vector2 location)
		{
			if (deadKoopaTroopa == null)
			{
				deadKoopaTroopa = new Sprite(false, true, location, koopaTroopaSprites, 1, 3, 2, 2);
				return deadKoopaTroopa;
			}
			else return deadKoopaTroopa;
		}
	}
}
