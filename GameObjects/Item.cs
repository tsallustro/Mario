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
    public class Item : IItem
    {
        private ISprite sprite;
        private IItemState itemState;
        private ItemSpriteFactory spriteFactory;
        private Vector2 velocity;
        private Vector2 location;

        public Item(Vector2 position)
        {
            spriteFactory = ItemSpriteFactory.Instance;
            this.location = position;
            sprite = spriteFactory.CreateCoin(location);
            itemState = new CoinState(this);
            velocity = new Vector2(0, 0);
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
        public void Update(GameTime gameTime)
        {
            sprite = spriteFactory.GetCurrentSprite(sprite.location, itemState);
            sprite.Update();
        }

        //Draw Item
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.location = location;
            sprite.Draw(spriteBatch, false);
        }

    }
}
