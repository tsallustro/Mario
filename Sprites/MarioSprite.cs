using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OUpdater;
using Game1;

/* This doesn't work right now. I'm cleaning at the moment. There was problem uploading it, so I'm reworking on it to get it to work */

namespace Sprites
{
    public class IdlingMarioSprite : Sprite
    {
        public IdlingMarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, 0, 0)
        {
        }
    }
        public class CrouchingMarioSprite : Sprite
    {
        public CrouchingMarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, 1, 1)
        {
        }
    }
    public class WalkingMarioSprite : Sprite
    {
        public WalkingMarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, 2, 3)
        {
        }

    }
    public class RunningMarioSprite : Sprite
    {
        public RunningMarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, 3, 4)
        {
        }

    }
    public class JumpingMarioSprite : Sprite
    {
        public JumpingMarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, 3, 5)
        {
        }

    }
    public class FallingMarioSprite : Sprite
    {
        public FallingMarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, 12, 12)
        {
        }

    }
    public class DyingMarioSprite : Sprite
    {
        public DyingMarioSprite(ObjectUpdater OU, Vector2 Location, MarioGame game)
            : base(OU, true, Location, game.Content.Load<Texture2D>("Mario"), 7, 14, 13, 13)
        {
        }

    }


}

