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
        Vector2 velocity { get; set; }
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

        private Rectangle Box
        {
            get { return new Rectangle((int)position.X, (int)position.Y, Width, Height); }
            
        }
        public GameObject(Texture2D texture)
        {
            _texture = texture;
        }

        public abstract void Update(GameTime GameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        //Methods
        public void Add(List<IGameObject> objs)
        {
            objs.Add(this);
        }
        public abstract void Animate();


        // Collision methods?
        public bool RightCollision(GameObject obj)
        {
            if (this.Box.Right + velocity.X > obj.Box.Left &&
                this.Box.Top > obj.Box.Bottom &&
                this.Box.Bottom < obj.Box.Top)
            {
                return true;
            } else { return false; }
        }
        public bool LeftCollision(GameObject obj)
        {
            if (this.Box.Left - velocity.X < obj.Box.Right &&
                this.Box.Top > obj.Box.Bottom &&
                this.Box.Bottom < obj.Box.Top)
            {
                return true;
            }
            else { return false; }
        }
        public bool TopCollision(GameObject obj)
        {
            if (this.Box.Top + velocity.Y > obj.Box.Bottom &&
                this.Box.Right > obj.Box.Left &&
                this.Box.Left < obj.Box.Right)
            {
                return true;
            }
            else { return false; }
        }
        public bool BottomCollision(GameObject obj)
        {
            if (this.Box.Bottom + velocity.Y > obj.Box.Top &&
                this.Box.Right > obj.Box.Left &&
                this.Box.Left < obj.Box.Right)
            {
                return true;
            }
            else { return false; }
        }

    }
}
