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
		public Goomba(ObjectUpdater OU, bool IsVisible, Vector2 Location,Texture2D texture)
			: base(OU, IsVisible, Location, texture, 1, 3, 0, 1)
		{
		}
    }

	public class KoopaTroopa : Sprite
	{
		public KoopaTroopa(ObjectUpdater OU, bool IsVisible, Vector2 Location, Texture2D texture)
			: base(OU, IsVisible, Location, texture, 1, 3, 0, 1)
		{
		}
	}
}
