using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Game1;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Sprites
{
    public class Goomba : Sprite
	{
		public Goomba(bool IsVisible, MarioGame game, Vector2 Location)
			: base(IsVisible, Location, game.Content.Load<Texture2D>("Goomba"), 1, 3, 0, 1)
		{
		}
    }

	public class KoopaTroopa : Sprite
	{
		public KoopaTroopa(bool IsVisible, MarioGame game, Vector2 Location)
			: base(IsVisible, Location, game.Content.Load<Texture2D>("KoopaTroopa"), 1, 3, 0, 1)
		{
		}
	}
}
