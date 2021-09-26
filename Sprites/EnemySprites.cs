using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OUpdater;
using Game1;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Sprites
{
    public class Goomba : Sprite
	{
		public Goomba(ObjectUpdater OU, bool IsVisible, MarioGame game, Vector2 Location)
			: base(OU, IsVisible, Location, game.Content.Load<Texture2D>("KoopaTroopa"), 1, 3, 0, 1)
		{
		}
    }

	public class KoopaTroopa : Sprite
	{
		public KoopaTroopa(ObjectUpdater OU, bool IsVisible, MarioGame game, Vector2 Location)
			: base(OU, IsVisible, Location, game.Content.Load<Texture2D>("KoopaTroopa"), 1, 3, 0, 1)
		{
		}
	}
}
