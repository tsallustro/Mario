using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
namespace GameObjects
{
	public interface IBoss : IGameObject
	{
		public void Damage();
		public void MoveRight();
		public void MoveLeft();
		public void MoveUp();
		public void MoveDown();
		public void Attack(GameTime gametime);
		public bool IsDead();

	}
}
