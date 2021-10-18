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
        private readonly int emergingVelocity = -30;

        protected readonly int numberOfSpritesOnSheet = 9;
        protected IItemState itemState;
        protected ItemSpriteFactory spriteFactory;
        protected bool isVisible = false;
        protected bool isEmergingFromBlock = false;
        protected Vector2 initialPosition;

        public Item(Vector2 position)
            : base(position, new Vector2(0, 0), new Vector2(0, 0))
        {
            initialPosition = position;
        }

        // This constructor should be used when creating STATIONARY items for testing
        public Item(Vector2 position, Vector2 velocity, Vector2 acceleration) 
            : base(position, velocity, acceleration)
        {
            initialPosition = position;
        }

        public IItemState GetItemState()
        {
            return itemState;
        }

        public void SetItemState(IItemState itemState)
        {
            this.itemState = itemState;
        }

        public bool GetVisibility()
        {
            return isVisible;
        }

        public void SetVisibility(bool isVisible)
        {
            this.isVisible = isVisible;
        }

        public void MakeVisibleAndEmerge()
        {
            this.isVisible = true;
            isEmergingFromBlock = true;
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

            if (Collidee is Mario)
            {
                Sprite.ToggleVisibility();
            }

        }

        //Update all items
        public override void Update(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Velocity += Acceleration * timeElapsed;
            Position += Velocity * timeElapsed;

            if (isEmergingFromBlock)
            {
                SetYVelocity(emergingVelocity);

                if (Sprite.texture != null && Position.Y <= initialPosition.Y - Sprite.texture.Height)
                {
                    SetYVelocity(0);
                    isEmergingFromBlock = false;
                }
            }

            Sprite = spriteFactory.GetCurrentSprite(Position, itemState);
            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            Sprite.Update();
        }

        //Draw Item
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                Sprite = spriteFactory.GetCurrentSprite(Sprite.location, itemState);
                Sprite.location = Position;
                Sprite.Draw(spriteBatch, false);
                DrawAABBIfVisible(Color.Green, spriteBatch);
            }
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
