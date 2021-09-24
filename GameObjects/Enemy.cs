using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
namespace GameObjects
{
	public interface Enemy
	{

		//Update enemy
		public void Update();

		//Draw enemy
		public void Draw(SpriteBatch SpriteBatch);

		//This interface will need more methods as the project grows and the needs of all enemies change. -Tony
	}
}
