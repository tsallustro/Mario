using Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;


namespace GameObjects
{
    /*
     * Since Items have different effects, we will likely need separate
     * concrete classes that extend this class, and we can make this into
     * an abstract class.
     */
    public class Item : GameObject, IItem
    {
        private readonly int boundaryAdjustment = -3;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */
        private readonly int numberOfSpritesOnSheet = 9;
        private IItemState itemState;
        private ItemSpriteFactory spriteFactory;

        public Item(Vector2 position)
            : base(position, new Vector2(0, 0), new Vector2(0, 0))
        {
            spriteFactory = ItemSpriteFactory.Instance;
            Sprite = spriteFactory.CreateCoin(position);
            itemState = new CoinState(this);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
        }

        public IItemState GetItemState()
        {
            return this.itemState;
        }

        public void SetItemState(IItemState itemState)
        {
            this.itemState = itemState;
        }

        //Update all items
        public override void Update(GameTime gameTime)
        {
            Sprite = spriteFactory.GetCurrentSprite(Sprite.location, itemState);
            Sprite.Update();
        }

        //Draw Item
        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.location = Position;
            Sprite.Draw(spriteBatch, false);

            // Prepare AABB visualization
            int lineWeight = 2;
            Color lineColor = Color.Green;
            Texture2D boundary = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            boundary.SetData(new[] { Color.White });

            /* Draw rectangle for the AABB visualization */
            spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X, (int)AABB.Location.Y + lineWeight, lineWeight, AABB.Height - 2 * lineWeight), lineColor);               // left
            spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X, (int)AABB.Location.Y, AABB.Width - lineWeight, lineWeight), lineColor);                               // top
            spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X + AABB.Width - lineWeight, (int)AABB.Location.Y, lineWeight, AABB.Height - lineWeight), lineColor);  // right
            spriteBatch.Draw(boundary, new Rectangle((int)AABB.Location.X, (int)AABB.Location.Y + AABB.Height - lineWeight, AABB.Width, lineWeight), lineColor);  // bottom
        }

    }
}
