using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Game1;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Sprites
{
    public class Flower : Sprite
	{
		public Flower(bool IsVisible, MarioGame game, Vector2 Location)
			: base(IsVisible, Location, game.Content.Load<Texture2D>("Items"), 1, 5, 2, 2)
		{
		}
    }

	public class Coin : Sprite
	{
		public Coin(bool IsVisible, MarioGame game, Vector2 Location)
			: base(IsVisible, Location, game.Content.Load<Texture2D>("Items"), 1, 5, 4, 4)
		{
		}
	}

	public class Mushroom : Sprite
	{
		public Mushroom(bool IsVisible, MarioGame game, Vector2 Location)
			: base(IsVisible, Location, game.Content.Load<Texture2D>("Items"), 1, 5, 0, 0)
		{
		}
	}

	public class MushroomOneUp : Sprite
	{
		public MushroomOneUp(bool IsVisible, MarioGame game, Vector2 Location)
			: base(IsVisible, Location, game.Content.Load<Texture2D>("Items"), 1, 5, 1, 1)
		{
		}
	}

	public class Star : Sprite
	{
		public Star(bool IsVisible, MarioGame game, Vector2 Location)
			: base(IsVisible, Location, game.Content.Load<Texture2D>("Items"), 1, 5, 3, 3)
		{
		}
	}
}
