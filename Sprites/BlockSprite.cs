using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Game1;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Sprites
{
    public class QuestionBlock : Sprite
	{
		public QuestionBlock(bool IsVisible, MarioGame game, Vector2 Location)
			: base(IsVisible, Location, game.Content.Load<Texture2D>("Blocks"), 1, 10, 6, 6)
		{
		}
    }

	public class UsedBlock : Sprite
	{
		public UsedBlock(bool IsVisible, MarioGame game, Vector2 Location)
			: base(IsVisible, Location, game.Content.Load<Texture2D>("Blocks"), 1, 10, 8, 8)
		{
		}
	}

	public class BrickBlock : Sprite
	{
		public BrickBlock(bool IsVisible, MarioGame game, Vector2 Location)
			: base(IsVisible, Location, game.Content.Load<Texture2D>("Blocks"), 1, 10, 0, 0)
		{
		}
	}

	public class FloorBlock : Sprite
	{
		public FloorBlock(bool IsVisible, MarioGame game, Vector2 Location)
			: base(IsVisible, Location, game.Content.Load<Texture2D>("Blocks"), 1, 10, 3, 3)
		{
		}
	}

	public class StairBlock : Sprite
	{
		public StairBlock(bool IsVisible, MarioGame game, Vector2 Location)
			: base(IsVisible, Location, game.Content.Load<Texture2D>("Blocks"), 1, 10, 2, 2)
		{
		}
	}
	public class HiddenBlock : Sprite
	{
		public HiddenBlock(bool IsVisible, MarioGame game, Vector2 Location)
			: base(IsVisible, Location, game.Content.Load<Texture2D>("Blocks"), 1, 10, 9, 9)
		{
		}
	}
}
