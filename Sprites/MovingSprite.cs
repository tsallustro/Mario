// Maxwell Ortwig

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace Game1
{
    public class MovingSprite : ISprite
    {
        private ObjectUpdater objectUpdater;
        public int rows { get; set; }
        public int columns { get; set; }
        public Texture2D texture { get; set; }
        public Vector2 location { get; set; }

        private int currentFrame = 0;
        private bool isVisible = true;
        private int movementDirection = 1;


        public MovingSprite(ObjectUpdater OU, bool IsVisible, Vector2 Location, Texture2D Texture, int Rows, int Columns)
        {
            objectUpdater = OU;
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
            
            // determines if sprite's visibility needs to be toggled and resets objectUpdater
            /*if (objectUpdater.movingSpriteVisibility)
            {
                isVisible = !isVisible;
                objectUpdater.movingSpriteVisibility = false;
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
