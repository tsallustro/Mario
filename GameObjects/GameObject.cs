﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace GameObjects
{
    public abstract class GameObject : IGameObject
    {
        protected Vector2 Position { get; set; }
        protected Vector2 Velocity { get; set; }
        protected Vector2 Acceleration { get; set; }
        protected ISprite Sprite { get; set; }
        protected Rectangle AABB { get; set; }
        public bool BorderIsVisible { get; set; } = false;

        private int Width
        {
            get { return Sprite.texture.Width; }
        }
        private int Height
        {
            get { return Sprite.texture.Height; }
        }

        public GameObject(Vector2 position, Vector2 velocity, Vector2 acceleration)
        {
            Position = position;
            Velocity = velocity;
            Acceleration = acceleration;
        }

        public abstract void Update(GameTime GameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public void SetXVelocity(float x)
        {
            this.Velocity = new Vector2(x, this.Velocity.Y);
        }

        public void SetYVelocity(float y)
        {
            this.Velocity = new Vector2(this.Velocity.X, y);
        }

        public Rectangle GetAABB()
        {
            return this.AABB;
        }

        // Collision methods?
        public bool RightCollision(IGameObject obj)
        {
            if (this.AABB.Right + Velocity.X > obj.GetAABB().Left &&
                this.AABB.Top > obj.GetAABB().Bottom &&
                this.AABB.Bottom < obj.GetAABB().Top &&
                this.AABB.Left < obj.GetAABB().Right)
            {
                return true;
            } else { return false; }
        }
        public bool LeftCollision(IGameObject obj)
        {
            if (this.AABB.Left - Velocity.X < obj.GetAABB().Right &&
                this.AABB.Top > obj.GetAABB().Bottom &&
                this.AABB.Bottom < obj.GetAABB().Top &&
                this.AABB.Right > obj.GetAABB().Left)
            {
                return true;
            }
            else { return false; }
        }
        public bool TopCollision(IGameObject obj)
        {
            if (this.AABB.Top - Velocity.Y > obj.GetAABB().Bottom &&
                this.AABB.Right > obj.GetAABB().Left &&
                this.AABB.Left < obj.GetAABB().Right &&
                this.AABB.Bottom > obj.GetAABB().Top)
            {
                return true;
            }
            else { return false; }
        }
        public bool BottomCollision(IGameObject obj)
        {
            if (this.AABB.Bottom + Velocity.Y > obj.GetAABB().Top &&
                this.AABB.Right > obj.GetAABB().Left &&
                this.AABB.Left < obj.GetAABB().Right &&
                this.AABB.Top < obj.GetAABB().Bottom)
            {
                return true;
            }
            else { return false; }
        }

        //React to collision. If Mario hits object, Mario stops.
        public void CheckCollsion(IGameObject obj)
        {
            if (RightCollision(obj) || LeftCollision(obj))
            {
                this.SetXVelocity((float)0);
            }
            if (TopCollision(obj) || BottomCollision(obj))
            {
                this.SetYVelocity((float)0);
            }

        }

    }
}
