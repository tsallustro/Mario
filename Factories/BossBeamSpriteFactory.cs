using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace CornetGame.Factories
{
    class BossBeamSpriteFactory
    {
		private static BossBeamSpriteFactory factoryInstance = new BossBeamSpriteFactory();

		public static BossBeamSpriteFactory Instance
		{
			get
			{
				return factoryInstance;
			}
		}

		private Texture2D beamTexture;
        public BossBeamSpriteFactory()
        {

        }
		public void LoadTextures(Game game)
		{
			beamTexture = game.Content.Load<Texture2D>("BossBeam");
		}
		public ISprite CreateBeam(Vector2 Location)
		{
			ISprite spr =  new Sprite(false, true, Location, beamTexture, 2, 3, 0, 5);
			((Sprite)spr).timePerFrame = 2;
			return spr;
		}
		
	}
}
