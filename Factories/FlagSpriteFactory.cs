using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprites;
using States;

namespace Factories
{
	public class FlagSpriteFactory
	{
		private Texture2D flagSprites;

		private static FlagSpriteFactory factoryInstance = new FlagSpriteFactory();

		private ISprite flag;
		private ISprite endingFlag;

		private int i;

		public static FlagSpriteFactory Instance
		{
			get
			{
				return factoryInstance;
			}
		}

		private FlagSpriteFactory()
		{
		}

		public void LoadTextures(Game game)
		{
			flagSprites = game.Content.Load<Texture2D>("Flagx50");
		}
		public ISprite CreateFlag(Vector2 location)
		{
			if(flag == null)
            {
				flag = new Sprite(false, true, location, flagSprites, 1, 50, 0, 0);
			    return flag;
			}
			else return flag;
		}

		public ISprite CreateEndingFlag(Vector2 location)
		{
			if (endingFlag == null)
			{
				endingFlag = new Sprite(false, true, location, flagSprites, 1, 50, 0, 49);
				return endingFlag;
			}
			else return endingFlag;
		}

	}
}
