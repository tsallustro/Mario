using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
namespace GameObjects
{
	public interface IEnemy
	{
		public void Stomped();
		public void Move();
		public void StayIdle();
		void Update(GameTime GameTime);
		void Draw(SpriteBatch spriteBatch);
	}
}
