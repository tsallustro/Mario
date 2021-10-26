using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
namespace GameObjects
{
	public interface IEnemy : IGameObject
	{
		public void Stomped();
		public Boolean IsDead();
		public void Move();
		public void StayIdle();
	}
}
