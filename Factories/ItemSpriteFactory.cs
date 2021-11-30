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
		private ISprite coin;
		private ISprite superMushroom;
		private ISprite oneUpMushroom;
		private ISprite fireFlower;
		private ISprite star;
		private ISprite bossPowerUp;

		public ItemSpriteFactory(Texture2D itemSprites)
		{
			this.itemSprites = itemSprites;
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
			}else if(itemState is BossPowerUpState)
            {
				return CreateBossPowerUp(location);
            }
			else
			{
				return CreateStar(location);
			}

		}
		
		public ISprite CreateCoin(Vector2 location)
        {
			if (coin != null)
			{
				return coin;
			}
			else
			{
				coin = new Sprite(false, true, location, itemSprites, 1, 10, 7, 8);
				return coin;
			}
		}

		public ISprite CreateSuperMushroom(Vector2 location)
		{
			if (superMushroom != null)
            {
				return superMushroom;
            } else
            {
				superMushroom = new Sprite(false, true, location, itemSprites, 1, 10, 0, 0);
				return superMushroom;
			}
		}

		public ISprite CreateOneUpMushroom(Vector2 location)
		{
			if (oneUpMushroom != null)
            {
				return oneUpMushroom;
            } else
            {
				oneUpMushroom = new Sprite(false, true, location, itemSprites, 1, 10, 1, 1);
				return oneUpMushroom;
			}
		}

		public ISprite CreateFireFlower(Vector2 location)
		{
			if (fireFlower != null)
            {
				return fireFlower;
            } else
            {
				fireFlower = new Sprite(false, true, location, itemSprites, 1, 10, 2, 2);
				return fireFlower;
			}
		}

		public ISprite CreateStar(Vector2 location)
		{
			if (star != null)
            {
				return star;
            } else
            {
				star = new Sprite(false, true, location, itemSprites, 1, 10, 3, 6);
				return star;
			}
		}

		public ISprite CreateBossPowerUp(Vector2 location)
        {
			if(bossPowerUp != null)
            {
				return bossPowerUp;
			}
            else
            {
				bossPowerUp = new Sprite(false, true, location, itemSprites, 1, 10, 9, 9);
				return bossPowerUp;
            }
        }
	}
}
