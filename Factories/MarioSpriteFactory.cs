using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprites;

namespace Factories
{
    public class MarioSpriteFactory
    {
        private Texture2D standardMarioSprites;
		private Texture2D superMarioSprites;
		private Texture2D fireMarioSprites;

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
			standardMarioSprites = game.Content.Load<Texture2D>("StandardMario");
			superMarioSprites = game.Content.Load<Texture2D>("SuperMario");
			fireMarioSprites = game.Content.Load<Texture2D>("FireMario");
		}

		public ISprite CreateIdleMario(Vector2 location)
		{
			return new Sprite(true, location, standardMarioSprites, 1, 14, 1, 1);
		}

		public ISprite CreateCrouchMario(Vector2 location)
        {
			return new Sprite(true, location, standardMarioSprites, 1, 14, 2, 2);
        }
	}
}
