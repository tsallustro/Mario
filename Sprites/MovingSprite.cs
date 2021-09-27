// Maxwell Ortwig

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sprites
{
    /*public class MovingSprite : ISprite
    {
        public int rows { get; set; }
        public int columns { get; set; }
        public Texture2D texture { get; set; }
        public Vector2 location { get; set; }
        public int movementDirection { get; set; } = 1;

        private int currentFrame = 0;
        private bool isVisible = true;

        public MovingSprite(bool IsVisible, Vector2 Location, Texture2D Texture, int Rows, int Columns)
        {
            bool isVisible = IsVisible;
            location = Location;
            texture = Texture;
            rows = Rows;
            columns = Columns;
        }
        public void Update()
        {
            // handles moving up and down
            if (location.Y < 25)
            {
                movementDirection = 1;
            } else if (location.Y > 200) {
                movementDirection = -1;
            }
            location += (new Vector2(0, movementDirection*5));
        }

        public void ToggleVisibility()
        {
            isVisible = !isVisible;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int width = texture.Width / columns;
            int height = texture.Height / rows;
            int row = currentFrame / columns;
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            if (isVisible) { spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White); }
        } 
    }*/
}
