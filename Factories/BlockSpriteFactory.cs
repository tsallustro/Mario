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
		private ISprite questionSprite;

		public BlockSpriteFactory(Texture2D blockSprites)
		{
			this.blockSprites = blockSprites;
		}

		/*
		 *  This method returns the correct sprite
		 */
		public ISprite GetCurrentSprite(Vector2 location, IBlockState blockState, Boolean collided)
		{

			if (blockState is QuestionBlockState)
			{
				return CreateQuestionBlock(location, collided);
			}
			else if (blockState is BumpedQuestionBlockState)
			{
				return CreateBumpedQuestionBlock(location, collided);
			}else if (blockState is UsedBlockState)
			{
				return CreateUsedBlock(location, collided);
			}else if (blockState is BumpedBrickBlockState)
			{
				return CreateBumpedBrickBlock(location, collided);
			}else if (blockState is BrickBlockState)
			{
				return CreateBrickBlock(location, collided);
			}
			else if (blockState is BrokenBrickBlockState)
			{
				return CreateBrokenBrickBlock(location, collided);
			}
			else if (blockState is FloorBlockState)
			{
				return CreateFloorBlock(location, collided);
			}
			else if (blockState is StairBlockState)
			{
				return CreateStairBlock(location, collided);
			}
			else
			{
				return CreateHiddenBlock(location, collided);
			}

		}

		public ISprite CreateHiddenBlock(Vector2 location, Boolean collided)
		{
			return new Sprite(collided, false, location, blockSprites, 1, 9, 8, 8);
		}
		public ISprite CreateUsedBlock(Vector2 location, Boolean collided)
		{
			return new Sprite(collided, true, location, blockSprites, 1, 9, 7, 7);
		}
		public ISprite CreateBumpedQuestionBlock(Vector2 location, Boolean collided)
		{
			return new Sprite(collided, true, location, blockSprites, 1, 9, 5, 6);
		}

		public ISprite CreateQuestionBlock(Vector2 location, Boolean collided)
		{
			if (questionSprite == null)
			{
				questionSprite = new Sprite(collided, true, location, blockSprites, 1, 9, 5, 6);
				return questionSprite;
			}
			else
			{
				return questionSprite;
			}
		}
		public ISprite CreateStairBlock(Vector2 location, Boolean collided)
		{
			return new Sprite(collided, true, location, blockSprites, 1, 9, 4, 4);
		}
		public ISprite CreateFloorBlock(Vector2 location, Boolean collided)
		{
			return new Sprite(collided, true, location, blockSprites, 1, 9, 3, 3);
		}

		public ISprite CreateBrokenBrickBlock(Vector2 location, Boolean collided)
		{
			return new Sprite(collided, true, location, blockSprites, 1, 9, 2, 2);
		}
		public ISprite CreateCrackedBrickBlock(Vector2 location, Boolean collided)
		{
			return new Sprite(collided, true, location, blockSprites, 1, 9, 1, 1);
		}
		public ISprite CreateBumpedBrickBlock(Vector2 location, Boolean collided)
		{
			return new Sprite(collided, true, location, blockSprites, 1, 9, 0, 0);
		}
		public ISprite CreateBrickBlock(Vector2 location, Boolean collided)
		{
			return new Sprite(collided, true, location, blockSprites, 1, 9, 0, 0);
		}
	}
}
