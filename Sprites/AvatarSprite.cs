using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1
{
	public class AvatarSprite : MovingAnimatedSprite
	{
		/* 
		 *  It seems like we can extend MovingAnimatedSprite
		 *  to get most of the functionality we need for Mario. We can add Mario-specific
		 *  functionality in this class so we can still use MovingAnimatedSprites elsewhere. 
		 */
		public AvatarSprite(ObjectUpdater OU, bool IsVisible, Vector2 Location, Texture2D Texture, int Rows, int Columns)
				: base(OU, IsVisible, Location, Texture, Rows, Columns)
		{

		}
	}
}
