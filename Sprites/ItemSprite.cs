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
    public class Flower : Sprite
	{
		public Flower(ObjectUpdater OU, bool IsVisible, MarioGame game, Vector2 Location)
			: base(OU, IsVisible, Location, game.Content.Load<Texture2D>("Items"), 1, 5, 2, 2)
		{
		}
    }

	public class Coin : Sprite
	{
		public Coin(ObjectUpdater OU, bool IsVisible, MarioGame game, Vector2 Location)
			: base(OU, IsVisible, Location, game.Content.Load<Texture2D>("Items"), 1, 5, 4, 4)
		{
		}
	}

	public class Mushroom : Sprite
	{
		public Mushroom(ObjectUpdater OU, bool IsVisible, MarioGame game, Vector2 Location)
			: base(OU, IsVisible, Location, game.Content.Load<Texture2D>("Items"), 1, 5, 0, 0)
		{
		}
	}

	public class MushroomOneUp : Sprite
	{
		public MushroomOneUp(ObjectUpdater OU, bool IsVisible, MarioGame game, Vector2 Location)
			: base(OU, IsVisible, Location, game.Content.Load<Texture2D>("Items"), 1, 5, 1, 1)
		{
		}
	}

	public class Star : Sprite
	{
		public Star(ObjectUpdater OU, bool IsVisible, MarioGame game, Vector2 Location)
			: base(OU, IsVisible, Location, game.Content.Load<Texture2D>("Items"), 1, 5, 3, 3)
		{
		}
	}
}
