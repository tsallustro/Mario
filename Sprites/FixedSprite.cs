// Maxwell Ortwig

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OUpdater;
namespace Sprites
{
    public class FixedSprite : ISprite
    {
        private ObjectUpdater objectUpdater;
        public int rows { get; set; }
        public int columns { get; set; }
        public Vector2 location { get; set; }
        public Texture2D texture { get; set; }
        private int currentFrame = 0;
        private bool isVisible = true;


        public FixedSprite(ObjectUpdater OU, bool IsVisible, Vector2 Location, Texture2D Texture, int Rows, int Columns)
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
            // DON'T NEED TO DO ANYTHING FOR FIXED NON-ANIMATED
            // determines if sprite's visibility needs to be toggled and resets objectUpdater
            /*if (objectUpdater.fixedSpriteVisibility)
            {
                isVisible = !isVisible;
                objectUpdater.fixedSpriteVisibility = false;
            }*/
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
