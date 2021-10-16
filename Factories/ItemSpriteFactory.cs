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
			return new Sprite(false, true, location, itemSprites, 1, 9, 7, 8);
		}

		public ISprite CreateSuperMushroom(Vector2 location)
		{
			return new Sprite(false, true, location, itemSprites, 1, 9, 0, 0);
		}

		public ISprite CreateOneUpMushroom(Vector2 location)
		{
			return new Sprite(false, true, location, itemSprites, 1, 9, 1, 1);
		}

		public ISprite CreateFireFlower(Vector2 location)
		{
			return new Sprite(false, true, location, itemSprites, 1, 9, 2, 2);
		}

		public ISprite CreateStar(Vector2 location)
		{
			return new Sprite(false, true, location, itemSprites, 1, 9, 3, 6);
		}
	}
}
