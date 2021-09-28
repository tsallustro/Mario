using Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;


namespace GameObjects
{
    public class Item
    {
        private ISprite sprite;
        private ItemState itemState;
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

        public ItemState GetItemState()
        {
            return this.itemState;
        }

        public void SetItemState(ItemState itemState)
        {
            this.itemState = itemState;
        }

        //Update all items
        public void Update(GameTime GameTime, GraphicsDeviceManager Graphics)
        {
            sprite = spriteFactory.GetCurrentSprite(sprite.location, itemState);
            sprite.Update();
        }

        //Draw Item
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.location = location;
            sprite.Draw(spriteBatch, true);
        }

    }
}
