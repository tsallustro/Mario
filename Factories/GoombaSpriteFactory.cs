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

		public ISprite GetCurrentSprite(Vector2 location, IGoombaState goombaState)
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
			return new Sprite(true, location, goombaSprites, 9, 15, 0, 0);
		}
		public ISprite CreateMovingGoomba(Vector2 location)
		{
			return new Sprite(true, location, goombaSprites, 9, 15, 0, 1);
		}
		public ISprite CreateStompedGoomba(Vector2 location)
		{
			return new Sprite(true, location, goombaSprites, 9, 15, 2, 2);
		}
		public ISprite CreateDeadGoomba(Vector2 location)
		{
			return new Sprite(true, location, goombaSprites, 9, 15, 0, 0);
		}
	}
}
