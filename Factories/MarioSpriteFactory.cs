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

		private static MarioSpriteFactory factoryInstance = new MarioSpriteFactory();

		// We don't want to continually instantiate more and more sprites.
		private ISprite standardIdle;
		private ISprite standardCrouch;
		private ISprite standardRun;
		private ISprite standardFall;
		private ISprite standardJump;
		private ISprite standardFlag;
		private ISprite superIdle;
		private ISprite superCrouch;
		private ISprite superRun;
		private ISprite superFall;
		private ISprite superJump;
		private ISprite superFlag;
		private ISprite fireIdle;
		private ISprite fireCrouch;
		private ISprite fireRun;
		private ISprite fireFall;
		private ISprite fireJump;
		private ISprite fireFlag;
		private ISprite dead;

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
				else if (actionState is JumpingState || actionState is FallingState) return CreateStandardJumpingMario(location);
				else if (actionState is FlagState) return CreateStandardFlagMario(location);
			} else if (powerState is SuperMario)
            {
				if (actionState is IdleState) return CreateSuperIdleMario(location);
				else if (actionState is CrouchingState) return CreateSuperCrouchingMario(location);
				else if (actionState is RunningState) return CreateSuperRunningMario(location);
				else if (actionState is JumpingState || actionState is FallingState) return CreateSuperJumpingMario(location);
				else if (actionState is FlagState) return CreateSuperFlagMario(location);
			} else if (powerState is FireMario)
            {
				if (actionState is IdleState) return CreateFireIdleMario(location);
				else if (actionState is CrouchingState) return CreateFireCrouchingMario(location);
				else if (actionState is RunningState) return CreateFireRunningMario(location);
				else if (actionState is JumpingState || actionState is FallingState) return CreateFireJumpingMario(location);
				else if (actionState is FlagState) return CreateFireFlagMario(location);
			}

			return CreateDeadMario(location);
		}

		public ISprite CreateStandardIdleMario(Vector2 location)
		{
			if (standardIdle == null)
			{
				standardIdle = new Sprite(false, true, location, standardMarioSprites, 1, 15, 0, 0);
				return standardIdle;
			}
			else return standardIdle;
			
		}

		public ISprite CreateStandardCrouchingMario(Vector2 location)
        {
			if (standardCrouch == null)
			{
				standardCrouch = new Sprite(false, true, location, standardMarioSprites, 1, 15, 1, 1);
				return standardCrouch;
			}
			else return standardCrouch;
        }

		public ISprite CreateStandardRunningMario(Vector2 location)
		{
			if (standardRun == null)
			{
				standardRun = new Sprite(false, true, location, standardMarioSprites, 1, 15, 2, 4);
				return standardRun;
			}
			else return standardRun;
		}

		public ISprite CreateStandardFallingMario(Vector2 location)
		{
			if (standardFall == null)
			{
				standardFall = new Sprite(false, true, location, standardMarioSprites, 1, 15, 5, 5);
				return standardFall;
			}
			else return standardFall;
		}

		public ISprite CreateStandardJumpingMario(Vector2 location)
        {
			if (standardJump == null)
			{
				standardJump = new Sprite(false, true, location, standardMarioSprites, 1, 15, 5, 5);
				return standardJump;
			}
			else return standardJump;
		}

		public ISprite CreateStandardFlagMario(Vector2 location)
        {
			if (standardFlag == null)
			{
				standardFlag = new Sprite(false, true, location, standardMarioSprites, 1, 15, 12, 12);
				return standardFlag;
			}
			else return standardFlag;
        }

		public ISprite CreateSuperIdleMario(Vector2 location)
		{
			if (superIdle == null)
			{
				superIdle = new Sprite(false, true, location, superMarioSprites, 1, 15, 0, 0);
				return superIdle;
			}
			else return superIdle;
		}

		public ISprite CreateSuperCrouchingMario(Vector2 location)
		{
			if (superCrouch == null)
			{
				superCrouch = new Sprite(false, true, location, superMarioSprites, 1, 15, 1, 1);
				return superCrouch;
			}
			else return superCrouch;
		}

		public ISprite CreateSuperRunningMario(Vector2 location)
		{
			if (superRun == null)
			{
				superRun = new Sprite(false, true, location, superMarioSprites, 1, 15, 2, 4);
				return superRun;
			}
			else return superRun;
		}

		public ISprite CreateSuperFallingMario(Vector2 location)
		{
			if (superFall == null)
			{
				superFall = new Sprite(false, true, location, standardMarioSprites, 1, 15, 5, 5);
				return superFall;
			}
			else return superFall;
		}
		public ISprite CreateSuperJumpingMario(Vector2 location)
		{
			if (superJump == null)
			{
				superJump = new Sprite(false, true, location, superMarioSprites, 1, 15, 5, 5);
				return superJump;
			}
			else return superJump;
		}

		public ISprite CreateSuperFlagMario(Vector2 location)
		{
			if (superFlag == null)
			{
				superFlag = new Sprite(false, true, location, superMarioSprites, 1, 15, 12, 12);
				return superFlag;
			}
			else return superFlag;
		}

		public ISprite CreateFireIdleMario(Vector2 location)
		{
			if (fireIdle == null)
			{
				fireIdle = new Sprite(false, true, location, fireMarioSprites, 1, 15, 0, 0);
				return fireIdle;
			}
			else return fireIdle;
		}

		public ISprite CreateFireCrouchingMario(Vector2 location)
		{
			if (fireCrouch == null)
			{
				fireCrouch = new Sprite(false, true, location, fireMarioSprites, 1, 15, 1, 1);
				return fireCrouch;
			}
			else return fireCrouch;
		}

		public ISprite CreateFireRunningMario(Vector2 location)
		{
			if (fireRun == null)
			{
				fireRun = new Sprite(false, true, location, fireMarioSprites, 1, 15, 2, 4);
				return fireRun;
			}
			else return fireRun;
		}

		public ISprite CreateFireFallingMario(Vector2 location)
		{
			if (fireFall == null)
			{
				fireFall = new Sprite(false, true, location, standardMarioSprites, 1, 15, 5, 5);
				return fireFall;
			}
			else return fireFall;
		}

		public ISprite CreateFireJumpingMario(Vector2 location)
		{
			if (fireJump == null)
			{
				fireJump = new Sprite(false, true, location, fireMarioSprites, 1, 15, 5, 5);
				return fireJump;
			}
			else return fireJump;
		}

		public ISprite CreateFireFlagMario(Vector2 location)
		{
			if (fireFlag == null)
			{
				fireFlag = new Sprite(false, true, location, fireMarioSprites, 1, 15, 12, 12);
				return fireFlag;
			}
			else return fireFlag;
		}

		public ISprite CreateDeadMario(Vector2 location)
        {
			if (dead == null)
			{
				dead = new Sprite(false, true, location, standardMarioSprites, 1, 15, 14, 14);
				return dead;
			}
			else return dead;
		}
	}
}
