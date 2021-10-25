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
        public Vector2 Acceleration { get; set; }
        public ISprite Sprite { get; set; }
        protected Rectangle AABB { get; set; }
        public bool BorderIsVisible { get; set; } = false;
        protected bool queuedForDeletion = false;
        private Vector2 AABBCollisionExtension { get; set; }

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
            AABBCollisionExtension = new Vector2(1, 1);
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

        public virtual void Update(GameTime gameTime)
        {
            if (Velocity.X > 100 || Velocity.X < -100) AABBCollisionExtension = new Vector2(2, AABBCollisionExtension.Y);
            else if (Velocity.X <= 100 && Velocity.X >= -100) AABBCollisionExtension = new Vector2(1, AABBCollisionExtension.Y);

            if (Velocity.Y > 100 || Velocity.Y < -100) AABBCollisionExtension = new Vector2(AABBCollisionExtension.X, 2);
            else if (Velocity.Y <= 100 && Velocity.Y >= -100) AABBCollisionExtension = new Vector2(AABBCollisionExtension.X, 1);
        }

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

        public void SetXAcceleration(float x)
        {
            this.Acceleration = new Vector2(x, this.Acceleration.Y);
        }

        public void SetYAcceleration(float y)
        {
            this.Acceleration = new Vector2(this.Acceleration.X, y);
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
        // Check for AABB collision
        //this.Top collide with obj.Bottom
        public bool TopCollision(IGameObject obj)
        {
            if (this.AABB.Top + AABBCollisionExtension.Y <= obj.GetAABB().Bottom &&
                this.AABB.Bottom > obj.GetAABB().Bottom &&
                this.AABB.Left < obj.GetAABB().Right &&
                this.AABB.Right > obj.GetAABB().Left)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //this.Bottom collide with obj.Top
        public bool BottomCollision(IGameObject obj)
        {

            if (this.AABB.Top < obj.GetAABB().Top &&
                this.AABB.Bottom + AABBCollisionExtension.Y >= obj.GetAABB().Top &&
                this.AABB.Left < obj.GetAABB().Right &&
                this.AABB.Right > obj.GetAABB().Left)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //this.Left collide with obj.Right
        public bool LeftCollision(IGameObject obj)
        {

            if (this.AABB.Top < obj.GetAABB().Bottom &&
                this.AABB.Bottom > (obj.GetAABB().Top + 1) &&
                this.AABB.Left - AABBCollisionExtension.X <= obj.GetAABB().Right &&
                this.AABB.Right > obj.GetAABB().Right)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //this.Right collide with obj.Left
        public bool RightCollision(IGameObject obj)
        {

            if (this.AABB.Top < obj.GetAABB().Bottom && 
                this.AABB.Bottom > (obj.GetAABB().Top + 1) &&
                this.AABB.Left < obj.GetAABB().Left &&
                this.AABB.Right + AABBCollisionExtension.X >= obj.GetAABB().Left )
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public abstract void Damage();

        public abstract void Halt();

        public bool isQueuedForDeletion()
        {
            return queuedForDeletion;
        }
    }
}
