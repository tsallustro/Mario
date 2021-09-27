using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Factories
{
    public class MarioSpriteFactory
    {
        private Texture2D marioSpritesheet;

		private static MarioSpriteFactory factoryInstance = new MarioSpriteFactory();

		public static MarioSpriteFactory Instance
		{
			get
			{
				return factoryInstance;
			}
		}

		private MarioSpriteFactory()
		{
		}

		public void LoadTextures(Game game)
		{
			marioSpritesheet = game.Content.Load<Texture2D>("Mario");
		}
	}
}
