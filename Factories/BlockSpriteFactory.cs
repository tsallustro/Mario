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

		private static BlockSpriteFactory factoryInstance = new BlockSpriteFactory();

		// We don't want to continually instantiate more and more sprites.
		private ISprite questionBlock;
		private ISprite usedBlock;
		private ISprite brickBlock;
		private ISprite floorBlock;
		private ISprite stairBlock;
		private ISprite hiddenBlock;

		//timer
		public int i = 0;
		public static BlockSpriteFactory Instance
		{
			get
			{
				return factoryInstance;
			}
		}

		private BlockSpriteFactory()
		{
		}

		public void LoadTextures(Game game)
		{
			blockSprites = game.Content.Load<Texture2D>("Blocks");

		}

		/*
		 *  This method returns the correct sprite given the current action and
		 *  power-up states of Mario.
		 */
		public ISprite GetCurrentSprite(Vector2 location, IBlockState blockState)
		{

			if (blockState is BrickBlockState)
			{
				return CreateBrickBlock(location);
			}
			else if (blockState is BumpedBrickBlockState)
			{
				return CreateBumpedBrickBlock(location);
			}
			else if (blockState is BrokenBrickBlockState)
			{
				return CreateBrokenBrickBlock(location);
			}
			else if (blockState is QuestionBlock)
			{
				return CreateQuestionBlock(location);
			}
			else if (blockState is UsedBlock)
			{
				return CreateUsedBlock(location);
			}
			else if (blockState is HiddenBlock)
			{
				return CreateHiddenBlock(location);
			}
			else if (blockState is FloorBlock)
			{
				return CreateFloorBlock(location);
			}
			else
			{
				return CreateBrickBlock(location);
			}

		}

		public ISprite CreateQuestionBlock(Vector2 location)
		{
			if (questionBlock == null)
			{
				questionBlock = new Sprite(true, location, blockSprites, 1, 9, 6, 7);
				return questionBlock;
			}
			else return questionBlock;
		}

		public ISprite CreateUsedBlock(Vector2 location)
		{
			if (usedBlock == null)
			{
				usedBlock = new Sprite(true, location, blockSprites, 1, 9, 8, 8);
				return usedBlock;
			}
			else return usedBlock;
		}

		public ISprite CreateBrickBlock(Vector2 location)
		{
			if (brickBlock == null)
			{
				brickBlock = new Sprite(true, location, blockSprites, 1, 9, 0, 0);
				return brickBlock;
			}
			else return brickBlock;
		}
		public ISprite CreateBumpedBrickBlock(Vector2 location)
		{
			if (brickBlock == null)
			{
				
				if (i < 5)
                {
					location.Y += 1;
					brickBlock = new Sprite(true, location, blockSprites, 1, 9, 0, 0);
					i++;
				} else if (i > 5)
                {

                }
				brickBlock = new Sprite(true, location, blockSprites, 1, 9, 0, 0);
				return brickBlock;
			}
			else return brickBlock;
		}
		public ISprite CreateBrokenBrickBlock(Vector2 location)
		{
			if (brickBlock == null)
			{
				brickBlock = new Sprite(true, location, blockSprites, 1, 9, 0, 0);
				return brickBlock;
			}
			else return brickBlock;
		}
		public ISprite CreateFloorBlock(Vector2 location)
		{
			if (floorBlock == null)
			{
				floorBlock = new Sprite(true, location, blockSprites, 1, 9, 4, 4);
				return floorBlock;
			}
			else return floorBlock;
		}

		public ISprite CreateStairBlock(Vector2 location)
		{
			if (stairBlock == null)
			{
				stairBlock = new Sprite(true, location, blockSprites, 1, 9, 2, 2);
				return stairBlock;
			}
			else return stairBlock;
		}

		public ISprite CreateHiddenBlock(Vector2 location)
		{
			if (hiddenBlock == null)
			{
				hiddenBlock = new Sprite(true, location, blockSprites, 1, 9, 8, 8);
				return hiddenBlock;
			}
			else return hiddenBlock;
		}
	}
}
