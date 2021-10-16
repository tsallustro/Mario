using Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;


namespace GameObjects
{
    public abstract class Item : GameObject, IItem
    {
        protected readonly int boundaryAdjustment = -10;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */
        protected readonly int numberOfSpritesOnSheet = 9;
        protected IItemState itemState;
        protected ItemSpriteFactory spriteFactory;

        public Item(Vector2 position)
            : base(position, new Vector2(0, 0), new Vector2(0, 0))
        {
        }

        public IItemState GetItemState()
        {
            return this.itemState;
        }

        public void SetItemState(IItemState itemState)
        {
            this.itemState = itemState;
        }

        public override void Halt()
        {
            return;
        }

        public override void Damage()
        {
            return;
        }
        public override void Collision(int side, GameObject Collidee)
        {

            if (Collidee is Mario mario)
            {
                //Sprite.ToggleVisibility();
                //this.
            }

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
            DrawAABBIfVisible(Color.Green, spriteBatch);
        }
        

    }

    public class Coin : Item
    {
        public Coin(Vector2 position)
            : base(position)
        {
            spriteFactory = ItemSpriteFactory.Instance;
            Sprite = spriteFactory.CreateCoin(position);
            itemState = new CoinState(this);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
        }
    }

    public class FireFlower : Item
    {
        public FireFlower(Vector2 position)
            : base(position)
        {
            spriteFactory = ItemSpriteFactory.Instance;
            Sprite = spriteFactory.CreateFireFlower(position);
            itemState = new FireFlowerState(this);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
        }
    }

    public class SuperMushroom : Item
    {
        public SuperMushroom(Vector2 position)
            : base(position)
        {
            spriteFactory = ItemSpriteFactory.Instance;
            Sprite = spriteFactory.CreateSuperMushroom(position);
            itemState = new SuperMushroomState(this);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
        }
    }

    public class OneUpMushroom : Item
    {
        public OneUpMushroom(Vector2 position)
            : base(position)
        {
            spriteFactory = ItemSpriteFactory.Instance;
            Sprite = spriteFactory.CreateOneUpMushroom(position);
            itemState = new OneUpMushroomState(this);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
        }
    }

    public class Star : Item
    {
        public Star(Vector2 position)
            : base(position)
        {
            spriteFactory = ItemSpriteFactory.Instance;
            Sprite = spriteFactory.CreateStar(position);
            itemState = new StarState(this);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
        }
    }
}
