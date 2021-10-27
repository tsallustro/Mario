using Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;
using Collisions;

namespace GameObjects
{
    public abstract class Item : GameObject, IItem
    {
        protected readonly static int mushroomSpeed = 50;
        protected readonly static int defaultItemGravity = 255;
        
        protected readonly static int boundaryAdjustment = 0;
        protected float lastY;
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
        protected GameObject blockItemIsOn;
       

        public Item(Vector2 position, Texture2D itemSprites, Mario mario)
            : base(position, new Vector2(0, 0), new Vector2(0, 0))
        {
            initialPosition = position;
            spriteFactory = new ItemSpriteFactory(itemSprites);
            boundMario = mario;
            lastY = this.Position.Y;
        }


        // This constructor should be used when creating STATIONARY items for testing
        public Item(Vector2 position, Vector2 velocity, Vector2 acceleration, Texture2D itemSprites, Mario mario) 
            : base(position, velocity, acceleration)
        {
            initialPosition = position;
            spriteFactory = new ItemSpriteFactory(itemSprites);
            boundMario = mario;
            lastY = this.Position.Y;
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

            if (Collidee is Mario && CanBePickedUp())
            {
                this.queuedForDeletion = true;
            }

            //Make blocks change direction with blocking entity
            if ((side == CollisionHandler.RIGHT || side == CollisionHandler.LEFT) && (Collidee is Block || Collidee is WarpPipe) && Velocity.X != 0)
            {
                SetXVelocity(-1 * Velocity.X);
            }

            //Make items stop falling when they hit a block
            if(isFinishedEmerging && side == CollisionHandler.BOTTOM && Collidee is Block || Collidee is WarpPipe)
            {
                GameObject block = Collidee;

                blockItemIsOn = block;
                this.SetYVelocity(0);
                this.SetYAcceleration(0);

                //This makes corrections based on gravity. The constant is the difference in y position between frames without having this line
                this.Position = new Vector2(Position.X, Position.Y-0.005554199f);
            }

        }

        

        //Update all items
        public override void Update(GameTime gameTime)
        {
            lastY = this.Position.Y;
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

            if (isFinishedEmerging && blockItemIsOn != null && !BottomCollision(blockItemIsOn))
            {
                SetYAcceleration(defaultItemGravity);
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

        public bool CanBePickedUp()
        {
            return this.isFinishedEmerging;
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
                    SetXVelocity(-1 * mushroomSpeed);
                }
                else if (Velocity.X == 0)
                {
                    //Mushroom is to the left of mario
                    SetXVelocity(mushroomSpeed);
                }
            }
        }
    }

    public class OneUpMushroom : Item
    {

        
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
                    SetXVelocity(mushroomSpeed);
                }
                else if (Velocity.X == 0)
                {
                    //Mushroom is to the left of mario
                    SetXVelocity(-1* mushroomSpeed);
                }
            }
        }
    }

    public class Star : Item
    {

        protected readonly static int starXSpeed = 10, starInitialBounceSpeed = 90;
       

        public Star(Vector2 position, Texture2D itemSprites, Mario mario)
            : base(position, itemSprites, mario)
        {
            Sprite = spriteFactory.CreateStar(position);
            itemState = new StarState(this);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            
        }
        public override void Collision(int side, GameObject Collidee)
        {
            
            base.Collision(side, Collidee);
           
            //Bounce on collision with ground
            if (isFinishedEmerging && Collidee is Block)
            {
                if (side == CollisionHandler.BOTTOM)
                {
                    
                    this.SetYVelocity(-1 * starInitialBounceSpeed);
                }
                else if (side == CollisionHandler.TOP)
                {
                    
                    this.SetYVelocity(1 * starInitialBounceSpeed);
                }

            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (isFinishedEmerging)
            {
                if (Velocity.X == 0 && Position.X - boundMario.GetPosition().X > 0)
                {
                    //Star is to the right of mario
                    SetXVelocity(mushroomSpeed);
                }
                else if (Velocity.X == 0)
                {
                    //Star is to the left of mario
                    SetXVelocity(-1 * mushroomSpeed);
                }

             
            }

          }

    }
}
