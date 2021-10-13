using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprites;
using States;

namespace Factories
{
    public class ItemSpriteFactory
    {
        private Texture2D itemSprites;

		private static ItemSpriteFactory factoryInstance = new ItemSpriteFactory();
		
		// TODO - Since we'll have multiple items, DO NOT cache the sprites
		// We don't want to continually instantiate more and more sprites.
		private ISprite coin;
		private ISprite superMushroom;
		private ISprite oneUpMushroom;
		private ISprite fireFlower;
		private ISprite star;

		public static ItemSpriteFactory Instance
		{
			get
			{
				return factoryInstance;
			}
		}

		private ItemSpriteFactory()
		{
		}

		public void LoadTextures(Game game)
		{
			itemSprites = game.Content.Load<Texture2D>("Items");
		}

		/*
		 *  This method returns the correct sprite.
		 */
		
		public ISprite GetCurrentSprite(Vector2 location, IItemState itemState)
        {
			if (itemState is CoinState)
            {
				return CreateCoin(location);
			} else if (itemState is SuperMushroomState)
			{
				return CreateSuperMushroom(location);
			}
			else if (itemState is OneUpMushroomState)
			{
				return CreateOneUpMushroom(location);
			}
			else if (itemState is FireFlowerState)
			{
				return CreateFireFlower(location);
			}
			else
			{
				return CreateStar(location);
			}

		}
		
		public ISprite CreateCoin(Vector2 location)
        {
			if (coin == null)
			{
				coin = new Sprite(false, true, location, itemSprites, 1, 9, 7, 8);
				return coin;
			}
			else return coin;
        }
		public ISprite CreateSuperMushroom(Vector2 location)
		{
			if (superMushroom == null)
			{
				superMushroom = new Sprite(false, true, location, itemSprites, 1, 9, 0, 0);
				return superMushroom;
			}
			else return superMushroom;
		}
		public ISprite CreateOneUpMushroom(Vector2 location)
		{
			if (oneUpMushroom == null)
			{
				oneUpMushroom = new Sprite(false, true, location, itemSprites, 1, 9, 1, 1);
				return oneUpMushroom;
			}
			else return oneUpMushroom;
		}
		public ISprite CreateFireFlower(Vector2 location)
		{
			if (fireFlower == null)
			{
				fireFlower = new Sprite(false, true, location, itemSprites, 1, 9, 2, 2);
				return fireFlower;
			}
			else return fireFlower;
		}
		public ISprite CreateStar(Vector2 location)
		{
			if (star == null)
			{
				star = new Sprite(false, true, location, itemSprites, 1, 9, 3, 6);
				return star;
			}
			else return star;
		}
	}
}
