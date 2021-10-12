using System;
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

        public void DrawAABBIfVisible(Color color, SpriteBatch spriteBatch)
        {
            if (BorderIsVisible)
            {
                int lineWeight = 2;
                Texture2D boundary = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                boundary.SetData(new[] { Color.White });

                /* Draw rectangle for the AABB visualization */
                spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X, (int)AABB.Location.Y + lineWeight, lineWeight, AABB.Height - 2 * lineWeight), color);               // left
                spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X, (int)AABB.Location.Y, AABB.Width - lineWeight, lineWeight), color);                                 // top
                spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X + AABB.Width - lineWeight, (int)AABB.Location.Y, lineWeight, AABB.Height - lineWeight), color);      // right
                spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X, (int)AABB.Location.Y + AABB.Height - lineWeight, AABB.Width, lineWeight), color);                   // bottom
            }
        }

        public abstract void Update(GameTime GameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        //positive x velocity makes object go right
        public void SetXVelocity(float x)
        {
            this.Velocity = new Vector2(x, this.Velocity.Y);
        }

        // positive y velocity makes object go down
        public void SetYVelocity(float y)
        {
            this.Velocity = new Vector2(this.Velocity.X, y);
        }
        public Vector2 GetPosition()
        {
            return this.Position;
        }  
        public Vector2 GetVelocity()
        {
            return this.Velocity;
        }
        public Rectangle GetAABB()
        {
            return this.AABB;
        }



        // Collision methods?
        public virtual void Collision(int side, GameObject Collidee)
        {
            // Overide this method
        }

        public bool RightCollision(IGameObject obj)
        {

            if (this.AABB.Right + 1 >= obj.GetAABB().Left &&
            this.AABB.Top < obj.GetAABB().Bottom &&
            this.AABB.Bottom > obj.GetAABB().Top &&
            this.AABB.Left < obj.GetAABB().Left)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public bool LeftCollision(IGameObject obj)
        {

            if (this.AABB.Left + 1 <= obj.GetAABB().Right &&
            this.AABB.Top < obj.GetAABB().Bottom &&
            this.AABB.Bottom > obj.GetAABB().Top &&
            this.AABB.Right > obj.GetAABB().Right)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool TopCollision(IGameObject obj)
        {

            if (this.AABB.Top + 1 <= obj.GetAABB().Bottom &&
            this.AABB.Left < obj.GetAABB().Right &&
            this.AABB.Right > obj.GetAABB().Left &&
            this.AABB.Bottom > obj.GetAABB().Bottom)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool BottomCollision(IGameObject obj)
        {

            if (this.AABB.Bottom + 1 >= obj.GetAABB().Top &&
            this.AABB.Left < obj.GetAABB().Right &&
            this.AABB.Right > obj.GetAABB().Left &&
            this.AABB.Top < obj.GetAABB().Top)
            {
                return true;
            }
            else
            {
                return false;
            }               
            
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

        public abstract void Damage();

        public abstract void Halt();
    }
}
