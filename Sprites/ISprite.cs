// Maxwell Ortwig

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Sprites
{
    public interface ISprite
    {
        public int rows { get; set; }
        public int columns { get; set; }
        public Vector2 location { get; set; }
        public Texture2D texture { get; set; }

        // Passing a vector into this method may become needed in the future, should two sprites need to interact.
        public void Update();
        public void ToggleVisibility();
        public void Draw(SpriteBatch SpriteBatch);


    }
}
