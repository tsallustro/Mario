using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sprites
{
    public class MovingAnimatedSprite : ISprite
    {
        public int rows { get; set; }
        public int columns { get; set; }
        public Texture2D texture { get; set; }
        public Vector2 location { get; set; }

        private bool isVisible = true;

        // We'll need to change direction to a 2D vector at some point
        private int movementDirection = 1;
        private int currentFrame = 2;


        public MovingAnimatedSprite(bool IsVisible, Vector2 Location, Texture2D Texture, int Rows, int Columns)
        {
            isVisible = IsVisible;
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
            }
            else if (location.Y > 200)
            {
                movementDirection = -1;
            }
            location += (new Vector2(0, movementDirection * 5));

            // sets frame to be arm up if going up or standing if going down
            if (movementDirection == 1)
            {
                currentFrame = 2;
            } else {
                currentFrame = 1;
            }
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
    }
}
