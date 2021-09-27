// Maxwell Ortwig

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sprites
{
    public class FixedAnimatedSprite : ISprite
    {
        public int rows { get; set; }
        public int columns { get; set; }
        public Texture2D texture { get; set; }
        public Vector2 location { get; set; }

        private bool isVisible = true;
        private int currentFrame = 2;
        private int frameDelay = 10;
        private int frameDelayCount = 0;


        public FixedAnimatedSprite(bool IsVisible, Vector2 Location, Texture2D Texture, int Rows, int Columns)
        {
            isVisible = IsVisible;
            location = Location;
            texture = Texture;
            rows = Rows;
            columns = Columns;
        }
        public void Update()
        {
            frameDelayCount++;
            if (frameDelay == frameDelayCount) {
                frameDelayCount = 0;
                currentFrame++;
                if (currentFrame == 4)
                {
                    currentFrame = 2;
                }
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
