using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OUpdater;
using Game1;

/* This doesn't work right now. I'm cleaning at the moment. There was problem uploading it, so I'm reworking on it to get it to work */

namespace Sprites
{
    public class MarioSprite : Sprite
    {
        public MarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game, int initialFrame, int finalFrame)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, initialFrame, finalFrame)
        {
        }
    }
 }

