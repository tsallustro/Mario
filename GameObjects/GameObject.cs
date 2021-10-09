using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace GameObjects
{
    public abstract class GameObject: IGameObject
    {
        Texture2D _texture { get; set; }
        Vector2 position { get; set; }
        Vector2 velocity;
        Vector2 accelaration { get; set; }
        float speed { get; set; }
        ISprite sprite { get; set; }

        // Need Rectangle to recognize the boundary?
        private int Width
        {
            get { return _texture.Width; }
        }
        private int Height
        {
            get { return _texture.Height; }
        }

        public Rectangle Box
        {
            get { return new Rectangle((int)position.X, (int)position.Y, Width, Height); }
            
        }
        public GameObject(Texture2D texture)
        {
            _texture = texture;
        }

        public abstract void Update(GameTime GameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        //Add object to the list
        public void Add(List<IGameObject> objs)
        {
            objs.Add(this);
        }
        public abstract void Animate();

        public abstract void TakeDamage();

        public void SetXVelocity(float x)
        {
            this.velocity.X = x;
        }

        public void SetYVelocity(float y)
        {
            this.velocity.Y = y;
        }


        // Collision methods?
        public bool RightCollision(IGameObject obj)
        {
            if (this.Box.Right + velocity.X > obj.Box.Left &&
                this.Box.Top > obj.Box.Bottom &&
                this.Box.Bottom < obj.Box.Top &&
                this.Box.Left < obj.Box.Right)
            {
                return true;
            } else { return false; }
        }
        public bool LeftCollision(IGameObject obj)
        {
            if (this.Box.Left - velocity.X < obj.Box.Right &&
                this.Box.Top > obj.Box.Bottom &&
                this.Box.Bottom < obj.Box.Top &&
                this.Box.Right > obj.Box.Left)
            {
                return true;
            }
            else { return false; }
        }
        public bool TopCollision(IGameObject obj)
        {
            if (this.Box.Top + velocity.Y > obj.Box.Bottom &&
                this.Box.Right > obj.Box.Left &&
                this.Box.Left < obj.Box.Right &&
                this.Box.Bottom > obj.Box.Top)
            {
                return true;
            }
            else { return false; }
        }
        public bool BottomCollision(IGameObject obj)
        {
            if (this.Box.Bottom + velocity.Y > obj.Box.Top &&
                this.Box.Right > obj.Box.Left &&
                this.Box.Left < obj.Box.Right &&
                this.Box.Top < obj.Box.Bottom)
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
