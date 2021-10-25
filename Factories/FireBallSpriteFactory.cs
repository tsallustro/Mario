using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprites;

namespace Factories
{
	public class FireBallSpriteFactory
	{
		private Texture2D fireballSprites;
		private ISprite fireball;
		private static FireBallSpriteFactory factoryInstance = new FireBallSpriteFactory();

		public static FireBallSpriteFactory Instance
		{
			get
			{
				return factoryInstance;
			}
		}

		private FireBallSpriteFactory()
		{
		}
		public void LoadTextures(Game game)
		{
			fireballSprites = game.Content.Load<Texture2D>("fireball");
		}
		public ISprite CreateFireBall(Vector2 Location)
		{
			if(fireball == null)
            {
				fireball = new Sprite(false, true, Location, fireballSprites, 1, 4, 0, 3);
			    return fireball;
			}
			else return fireball;
		}
		public ISprite GetCurrentSprite(Vector2 Location)
		{
			fireball.location = Location;
			return fireball;
        }





    }
}
