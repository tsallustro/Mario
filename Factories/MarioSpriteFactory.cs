using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprites;
using States;

namespace Factories
{
    public class MarioSpriteFactory
    {
        private Texture2D standardMarioSprites;
		private Texture2D superMarioSprites;
		private Texture2D fireMarioSprites;
		private Texture2D deadMarioSprite;

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
			deadMarioSprite = game.Content.Load<Texture2D>("DeadMario");
		}

		/*
		 *  This method returns the correct sprite given the current action and
		 *  power-up states of Mario.
		 */
		public ISprite GetCurrentSprite(Vector2 location, IMarioActionState actionState, IMarioPowerState powerState)
        {
			if (powerState is StandardMario)
            {
				if (actionState is IdleState) return CreateStandardIdleMario(location);
				else if (actionState is CrouchingState) return CreateStandardCrouchingMario(location);
				else if (actionState is RunningState) return CreateStandardRunningMario(location);
				else if (actionState is JumpingState) return CreateStandardJumpingMario(location);
			} else if (powerState is SuperMario)
            {
				if (actionState is IdleState) return CreateSuperIdleMario(location);
				else if (actionState is CrouchingState) return CreateSuperCrouchingMario(location);
				else if (actionState is RunningState) return CreateSuperRunningMario(location);
				else if (actionState is JumpingState) return CreateSuperJumpingMario(location);
			} else if (powerState is FireMario)
            {
				if (actionState is IdleState) return CreateFireIdleMario(location);
				else if (actionState is CrouchingState) return CreateFireCrouchingMario(location);
				else if (actionState is RunningState) return CreateFireRunningMario(location);
				else if (actionState is JumpingState) return CreateFireJumpingMario(location);
			}

			return CreateDeadMario(location);
		}

		public ISprite CreateStandardIdleMario(Vector2 location)
		{
			return new Sprite(true, location, standardMarioSprites, 1, 14, 0, 0);
		}

		public ISprite CreateStandardCrouchingMario(Vector2 location)
        {
			return new Sprite(true, location, standardMarioSprites, 1, 14, 1, 1);
        }

		public ISprite CreateStandardRunningMario(Vector2 location)
        {
			return new Sprite(true, location, standardMarioSprites, 1, 14, 2, 4);
		}

		public ISprite CreateStandardJumpingMario(Vector2 location)
        {
			return new Sprite(true, location, standardMarioSprites, 1, 14, 5, 5);
		}

		public ISprite CreateSuperIdleMario(Vector2 location)
		{
			return new Sprite(true, location, superMarioSprites, 1, 14, 0, 0);
		}

		public ISprite CreateSuperCrouchingMario(Vector2 location)
		{
			return new Sprite(true, location, superMarioSprites, 1, 14, 1, 1);
		}

		public ISprite CreateSuperRunningMario(Vector2 location)
		{
			return new Sprite(true, location, superMarioSprites, 1, 14, 2, 4);
		}

		public ISprite CreateSuperJumpingMario(Vector2 location)
		{
			return new Sprite(true, location, superMarioSprites, 1, 14, 5, 5);
		}

		public ISprite CreateFireIdleMario(Vector2 location)
		{
			return new Sprite(true, location, fireMarioSprites, 1, 14, 0, 0);
		}

		public ISprite CreateFireCrouchingMario(Vector2 location)
		{
			return new Sprite(true, location, fireMarioSprites, 1, 14, 1, 1);
		}

		public ISprite CreateFireRunningMario(Vector2 location)
		{
			return new Sprite(true, location, fireMarioSprites, 1, 14, 2, 4);
		}

		public ISprite CreateFireJumpingMario(Vector2 location)
		{
			return new Sprite(true, location, fireMarioSprites, 1, 14, 5, 5);
		}
		
		public ISprite CreateDeadMario(Vector2 location)
        {
			return new Sprite(true, location, deadMarioSprite, 1, 1, 0, 0);
		}
	}
}
