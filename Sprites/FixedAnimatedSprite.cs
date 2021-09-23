// Maxwell Ortwig

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace Game1
{
    public class FixedAnimatedSprite : ISprite
    {
        private ObjectUpdater objectUpdater;
        public int rows { get; set; }
        public int columns { get; set; }
        public Texture2D texture { get; set; }
        public Vector2 location { get; set; }

        private bool isVisible = true;
        private int currentFrame = 2;
        private int frameDelay = 10;
        private int frameDelayCount = 0;


        public FixedAnimatedSprite(ObjectUpdater OU, bool IsVisible, Vector2 Location, Texture2D Texture, int Rows, int Columns)
        {

            objectUpdater = OU;
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

            // determines if sprite's visibility needs to be toggled and resets objectUpdater
            if (objectUpdater.fixedAnimatedSpriteVisibility)
            {
                isVisible = !isVisible;
                objectUpdater.fixedAnimatedSpriteVisibility = false;
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

            // draws sprite visible or transparent
            if (isVisible)
            {
                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
            } else {
                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.Transparent);
            }
        }
    }
}
