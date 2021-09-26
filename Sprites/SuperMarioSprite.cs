using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OUpdater;
using Game1;

/* This doesn't work right now. I'm cleaning at the moment. There was problem uploading it, so I'm reworking on it to get it to work */

namespace Sprites
{
    public class SuperMarioSprite : Sprite
    {
        public SuperMarioSprite(ObjectUpdater OU, bool IsVisible, Vector2 Location, Texture2D Texture, int Rows, int Columns, int initialFrame, int finalFrame)
            : base(OU, IsVisible, Location, Texture, 7, 14, initialFrame, finalFrame)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Twice the size for every mario other than little mario
            int width = texture.Width / columns;
            int height = 2* texture.Height / rows;
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
    public class IdlingSuperMarioSprite : SuperMarioSprite
    {
        public IdlingSuperMarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, 14, 0)
        {
        }
    }
    public class CrouchingSuperMarioSprite : SuperMarioSprite
    {
        public CrouchingSuperMarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, 15, 1)
        {
        }
    }
    public class WalkinggSuperMarioSprite : SuperMarioSprite
    {
        public WalkinggSuperMarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, 16, 17)
        {
        }

    }
    public class RunningSuperMarioSprite : SuperMarioSprite
    {
        public RunningSuperMarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, 16, 18)
        {
        }

    }
    public class JumpingSuperMarioSprite : SuperMarioSprite
    {
        public JumpingSuperMarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, 18, 19)
        {
        }

    }
    public class FallingSuperMarioSprite : SuperMarioSprite
    {
        public FallingSuperMarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, 26, 26)
        {
        }

    }
    public class DyingSuperMarioSprite : SuperMarioSprite
    {
        public DyingSuperMarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, 27, 27)
        {
        }

    }
}

