using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OUpdater;

// Base sprites class for all sprites
namespace Sprites
{
    public class Sprite : ISprite
    {

        public ObjectUpdater objectUpdater;
        public int rows { get; set; }
        public int columns { get; set; }
        public Texture2D texture { get; set; }
        public Vector2 location { get; set; }

        public bool isVisible = true;
          
        // We'll need to change direction to a 2D vector at some point
        public int movementDirection = 1;
        //To get the animation working
        public int currentFrame = 0;
        //Set the range of frames used in sprite sheet
        public int InitialFrame { get; set; }
        public int FinalFrame { get; set; }
        //Timer to slow down animation
        public int timer = 0;

        public Sprite(ObjectUpdater OU, bool IsVisible, Vector2 Location, Texture2D Texture, int Rows, int Columns, int initialFrame, int finalFrame)
        {
            
            objectUpdater = OU;
            isVisible = IsVisible;
            location = Location;
            texture = Texture;
            rows = Rows;
            columns = Columns;
            InitialFrame = initialFrame;
            FinalFrame = finalFrame;

        }
        public void Update()
        {

            //ChangeDirection();
            //Move();
            //Jump();
            //ToggleInvisOrNot();

        }

        private void ToggleInvisOrNot()
        {
            // determines if sprite's visibility needs to be toggled and resets objectUpdater
            if (objectUpdater.movingAnimatedSpriteVisibility)
            {
                isVisible = !isVisible;
                objectUpdater.movingAnimatedSpriteVisibility = false;
            }
        }

        // Sets Frame Animation. @param int initialFrame int finalFrame
        public void Animation()
        {
            if (timer > 5)
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
            if (location.X > 500)
            {
                movementDirection = -1;
            }
            else if (location.X < 0)
            {
                movementDirection = 1;
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
            }
            else
            {
                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.Transparent);
            }
        }
     }
 }

