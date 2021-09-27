using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Base sprites class for all sprites
namespace Sprites
{
    public class Sprite : ISprite
    {
        public int rows { get; set; }
        public int columns { get; set; }
        public Texture2D texture { get; set; }
        public Vector2 location { get; set; }

        public bool isVisible = true;

        // We'll need to change direction to a 2D vector at some point
        public int movementDirection { get; set; } = 1;
        //To get the animation working
        public int currentFrame = 0;
        //Set the range of frames used in sprite sheet
        public int InitialFrame { get; set; }
        public int FinalFrame { get; set; }
        //Timer to slow down animation
        public int timer = 0;

        public Sprite(bool IsVisible, Vector2 Location, Texture2D Texture, int Rows, int Columns, int initialFrame, int finalFrame)
        {
            isVisible = IsVisible;
            location = Location;
            texture = Texture;
            rows = Rows;
            columns = Columns;
            currentFrame = initialFrame;
            InitialFrame = initialFrame;
            FinalFrame = finalFrame;
        }

        public void Update()
        {
            //Animation();
            //ChangeDirection();
            //Move();
            //Jump();
            //ToggleInvisOrNot();

        }

        // Sets Frame Animation. @param int initialFrame int finalFrame
        public void Animation()
        {
            if (timer > 6)
            {
                currentFrame++;
                if (currentFrame == FinalFrame + 1)
                    currentFrame = InitialFrame;
                timer = 0;
            }
            timer++;
        }

        //Move Method
        public void Move()
        {
            location += (new Vector2(movementDirection * 1, 0));
            Animation();
        }

        //Jump Method
        public void Jump()
        {
            location += (new Vector2(0, 1));
            Animation();
        }

        //Change Direction when it reaches certain point.
        //Suggestion: Set boundary of the screen as the point of direction change.
        public void ChangeDirection()
        {
            movementDirection = -movementDirection;
        }
        
        public void ToggleVisibility()
        {
            isVisible = !isVisible;
        }
        
        public virtual void Draw(SpriteBatch spriteBatch, bool left)
        {
            int width = texture.Width / columns;
            int height = texture.Height / rows;
            int row = currentFrame / columns;
            int column = currentFrame % columns;
            
            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);
            
            if (isVisible) { 
                if (!left) spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
                else spriteBatch.Draw(texture, destinationRectangle, sourceRectangle,
                    Color.White, 0f, new Vector2(0f, 0f), SpriteEffects.FlipHorizontally, 0f);
            }
        }
     }
 }

