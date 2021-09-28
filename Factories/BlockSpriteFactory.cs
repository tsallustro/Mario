using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprites;
using States;

namespace Factories
{
	public class BlockSpriteFactory
	{
		private Texture2D blockSprites;

		public BlockSpriteFactory(Texture2D blockSprites)
		{
			this.blockSprites = blockSprites;
		}

		/*
		 *  This method returns the correct sprite
		 */
		public ISprite GetCurrentSprite(Vector2 location, IBlockState blockState)
		{

			if (blockState is QuestionBlockState)
			{
				return CreateQuestionBlock(location);
			}else if (blockState is UsedBlockState)
			{
				return CreateUsedBlock(location);
			}else if (blockState is BrickBlockState)
			{
				return CreateBrickBlock(location);
			}else if (blockState is BrokenBrickBlockState)
            {
				return CreateBrokenBrickBlock(location);
            }
			else if (blockState is FloorBlockState)
			{
				return CreateFloorBlock(location);
			}else if (blockState is StairBlockState)
			{
				return CreateStairBlock(location);
			}else
			{
				return CreateHiddenBlock(location);
			}

		}

		public ISprite CreateQuestionBlock(Vector2 location)
		{
			return new Sprite(true, location, blockSprites, 1, 8, 4, 5);
		}

		public ISprite CreateUsedBlock(Vector2 location)
		{
			return new Sprite(true, location, blockSprites, 1, 8, 6, 6);
		}

		public ISprite CreateBrickBlock(Vector2 location)
		{
			return new Sprite(true, location, blockSprites, 1, 8, 0, 0);
		}

		public ISprite CreateFloorBlock(Vector2 location)
		{
			return new Sprite(true, location, blockSprites, 1, 8, 2, 2);
        }

		public ISprite CreateStairBlock(Vector2 location)
		{
			return new Sprite(true, location, blockSprites, 1, 8, 3, 3);
        }

		public ISprite CreateHiddenBlock(Vector2 location)
		{
			return new Sprite(false, location, blockSprites, 1, 8, 7, 7);
		}

		public ISprite CreateBumpedBrickBlock(Vector2 location)
		{
			return new Sprite(true, location, blockSprites, 1, 8, 0, 0);
        }

		public ISprite CreateBrokenBrickBlock(Vector2 location)
		{
			return new Sprite(true, location, blockSprites, 1, 8, 1, 1);
        }	
	}
}
