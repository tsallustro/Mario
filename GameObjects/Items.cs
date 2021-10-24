using Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;


namespace GameObjects
{
    public abstract class Item : GameObject, IItem
    {
        protected readonly int boundaryAdjustment = +5;
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
        protected bool isFinishedEmerging = false;
        protected Vector2 initialPosition;
        protected Mario boundMario;
       

        public Item(Vector2 position, Texture2D itemSprites, Mario mario)
            : base(position, new Vector2(0, 0), new Vector2(0, 0))
        {
            initialPosition = position;
            spriteFactory = new ItemSpriteFactory(itemSprites);
            boundMario = mario;
        }

        // This constructor should be used when creating STATIONARY items for testing
        public Item(Vector2 position, Vector2 velocity, Vector2 acceleration, Texture2D itemSprites, Mario mario) 
            : base(position, velocity, acceleration)
        {
            initialPosition = position;
            spriteFactory = new ItemSpriteFactory(itemSprites);
            boundMario = mario;
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

            if (Collidee is Mario && !isEmergingFromBlock)
            {
                this.queuedForDeletion = true;
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
                    isFinishedEmerging = true;
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
        public Coin(Vector2 position, Texture2D itemSprites, Mario mario)
            : base(position, itemSprites, mario)
        {
            Sprite = spriteFactory.CreateCoin(position);
            itemState = new CoinState(this);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            
        }
    }

    public class FireFlower : Item
    {
        public FireFlower(Vector2 position, Texture2D itemSprites, Mario mario)
            : base(position, itemSprites, mario)
        {
            Sprite = spriteFactory.CreateFireFlower(position);
            itemState = new FireFlowerState(this);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            
        }
    }

    public class SuperMushroom : Item
    {

        private static int superMushroomSpeed = 5;
        public SuperMushroom(Vector2 position, Texture2D itemSprites, Mario mario)
            : base(position, itemSprites, mario)
        {
            Sprite = spriteFactory.CreateSuperMushroom(position);
            itemState = new SuperMushroomState(this);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
           
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (isFinishedEmerging)
            {


                if (Velocity.X == 0 && Position.X - boundMario.GetPosition().X > 0)
                {
                    //Mushroom is to the right of mario
                    SetXVelocity(-1 * superMushroomSpeed);
                }
                else if (Velocity.X == 0)
                {
                    //Mushroom is to the left of mario
                    SetXVelocity(superMushroomSpeed);
                }
            }
        }
    }

    public class OneUpMushroom : Item
    {

        private static int oneUpMushroomSpeed = 1;
        public OneUpMushroom(Vector2 position, Texture2D itemSprites, Mario mario)
            : base(position, itemSprites, mario)
        {
            Sprite = spriteFactory.CreateOneUpMushroom(position);
            itemState = new OneUpMushroomState(this);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (isFinishedEmerging)
            {


                if (Velocity.X == 0 && Position.X - boundMario.GetPosition().X > 0)
                {
                    //Mushroom is to the right of mario
                    SetXVelocity(oneUpMushroomSpeed);
                }
                else if (Velocity.X == 0)
                {
                    //Mushroom is to the left of mario
                    SetXVelocity(-1*oneUpMushroomSpeed);
                }
            }
        }
    }

    public class Star : Item
    {
        public Star(Vector2 position, Texture2D itemSprites, Mario mario)
            : base(position, itemSprites, mario)
        {
            Sprite = spriteFactory.CreateStar(position);
            itemState = new StarState(this);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            
        }
    }
}
