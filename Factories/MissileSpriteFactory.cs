using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprites;

namespace Factories
{
	public class MissileSpriteFactory
	{
		private Texture2D missileSprites;
		private ISprite missile;
		private static MissileSpriteFactory factoryInstance = new MissileSpriteFactory();

		public static MissileSpriteFactory Instance
		{
			get
			{
				return factoryInstance;
			}
		}

		private MissileSpriteFactory()
		{
		}
		public void LoadTextures(Game game)
		{
			missileSprites = game.Content.Load<Texture2D>("Missile");
		}
		public ISprite CreateMissile(Vector2 Location)
		{
			if(missile == null)
            {
				missile = new Sprite(false, true, Location, missileSprites, 1, 4, 0, 3);
			    return missile;
			}
			else return missile;
		}
		public ISprite GetCurrentSprite(Vector2 Location)
		{
			missile.location = Location;
			return missile;
        }





    }
}
