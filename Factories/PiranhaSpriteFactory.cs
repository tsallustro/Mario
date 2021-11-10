using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprites;
using States;

namespace Factories
{
	public class PiranhaSpriteFactory
	{
		private Texture2D piranhaSprites;

		private static PiranhaSpriteFactory factoryInstance = new PiranhaSpriteFactory();

		private ISprite idlePiranha;
		private ISprite activePiranha;

		public static PiranhaSpriteFactory Instance
		{
			get
			{
				return factoryInstance;
			}
		}

		private PiranhaSpriteFactory()
		{
		}

		public void LoadTextures(Game game)
		{
			piranhaSprites = game.Content.Load<Texture2D>("Piranha");
		}

		/*
		 *  This method returns the correct sprite given the current action and
		 *  power-up states of Piranha.
		 */
		public ISprite GetCurrentSprite(Vector2 location, IEnemyState piranhaState)
		{
			if (piranhaState is IdlePiranhaState)
			{
				return CreateIdlePiranha(location);
			} else
            {
				return CreateActivePiranha(location);
			}
		}
		public ISprite CreateIdlePiranha(Vector2 location)
		{
			if (idlePiranha == null)
			{
				idlePiranha = new Sprite(false, true, location, piranhaSprites, 1, 8, 7, 7);
				return idlePiranha;
			}
			else return idlePiranha;
		}
		public ISprite CreateActivePiranha(Vector2 location)
		{
			if (activePiranha == null)
			{
				activePiranha = new Sprite(false, true, location, piranhaSprites, 1, 8, 0, 0);
				return activePiranha;
			}
			else return activePiranha;
		}
	}
}
