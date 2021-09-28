using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
namespace GameObjects
{
	public interface IEnemy
	{
		public void Update();
		public void Draw(SpriteBatch SpriteBatch);
		public void Stomped();
		public void Move();
		public void StayIdle();
	}
}
