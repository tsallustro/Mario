using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprites;
using States;

namespace Factories
{
    public class BowserSpriteFactory
    {
		private Texture2D bowserSprites;

		private static BowserSpriteFactory factoryInstance = new BowserSpriteFactory();

		// We don't want to continually instantiate more and more sprites.
		private ISprite idleBowser;
		private ISprite damagedOneBowser;
		private ISprite damagedTwoBowser;
		private ISprite deadBowser;

		private ISprite bowser;

		public static BowserSpriteFactory Instance
		{
			get
			{
				return factoryInstance;
			}
		}

		private BowserSpriteFactory()
		{
		}

		public void LoadTextures(Game game)
		{
			bowserSprites = game.Content.Load<Texture2D>("BowserSpriteSheet");
		}

		/*
		 *  This method returns the correct sprite given the current action and
		 *  power-up states of Mario.
		 */
		public ISprite GetCurrentSprite(Vector2 location, IBossState bowserState)
        {
			if (bowserState is IdleBowserState)
			{
				return CreateIdleBowser(location);

			} else if (bowserState is DamagedOneBowserState)
            {
				return CreateDamagedOneBowser(location);
			}
			else if (bowserState is DamagedTwoBowserState)
			{
				return CreateDamagedTwoBowser(location);
			}
			return CreateDeadBowser(location);
		}

		public ISprite CreateIdleBowser(Vector2 location)
		{
			if (idleBowser == null)
			{
				idleBowser = new Sprite(false, true, location, bowserSprites, 1, 10, 0, 2);
				return idleBowser;
			}
			else return idleBowser;
		}
		public ISprite CreateDamagedOneBowser(Vector2 location)
		{
			if (damagedOneBowser == null)
			{
				damagedOneBowser = new Sprite(false, true, location, bowserSprites, 1, 10, 3, 5);
				return damagedOneBowser;
			}
			else return damagedOneBowser;
		}
		public ISprite CreateDamagedTwoBowser(Vector2 location)
		{
			if (damagedTwoBowser == null)
			{
				damagedTwoBowser = new Sprite(false, true, location, bowserSprites, 1, 10, 6, 8);
				return damagedTwoBowser;
			}
			else return damagedTwoBowser;
		}
		public ISprite CreateDeadBowser(Vector2 location)
		{
			if (deadBowser == null)
			{
				deadBowser = new Sprite(false, true, location, bowserSprites, 1, 10, 9, 9);
				return deadBowser;
			}
			else return deadBowser;
		}

	}
}
