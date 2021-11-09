using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;
using Factories;
using Collisions;
using Sound;

namespace GameObjects
{
    public class Block : GameObject, IBlock
    {
        private readonly int boundaryAdjustment = 0;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */
        private readonly int numberOfSpritesOnSheet = 9;
        private IBlockState blockState;
        private Mario mario;
        private BlockSpriteFactory spriteFactory;
        private Vector2 originalLocation;
        private Boolean falling = false;
        private Boolean bumped = false;
        private List<IItem> items;

        public Block(Vector2 position, Texture2D blockSprites, Mario Mario)
            : base(position, new Vector2(0, 0), new Vector2(0, 0))
        {
            originalLocation = position;
            mario = Mario;
            spriteFactory = new BlockSpriteFactory(blockSprites);
            this.items = new List<IItem>();
            blockState = new BrickBlockState(this);
            Sprite = spriteFactory.CreateBrickBlock(position, false);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
        }

        // Future constructor for adding items to block
        public Block(Vector2 position, Texture2D blockSprites, Mario Mario, List<IItem> items)
            : base(position, new Vector2(0, 0), new Vector2(0, 0))
        {
            originalLocation = position;
            mario = Mario;
            spriteFactory = new BlockSpriteFactory(blockSprites);
            this.items = items;
            blockState = new BrickBlockState(this);
            Sprite = spriteFactory.CreateBrickBlock(position, false);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
        }

        public void SetLocation(Vector2 position)
        {
            Sprite.location = position;
        }
        public Vector2 GetLocation()
        {
            return Position;
        }
        public void SetFalling(Boolean Falling)
        {
            this.falling = Falling;
        }
        public Boolean GetFalling()
        {
            return this.falling;
        }
        public void SetBumped(Boolean bumped)
        {
            this.bumped = bumped;
        }
        public Boolean GetBumped()
        {
            return this.bumped;
        }

        public IBlockState GetBlockState()
        {
            return blockState;
        }
        public void SetBlockState(IBlockState blockState)
        {
            this.blockState = blockState;
        }

        public List<IItem> GetItems()
        {
            return this.items;
        }

        /*
         *  This method assumes that items.Count() > 0. Must check before calling!
         */
        public IItem RemoveItem()
        {
            IItem item = items[0];
            items.RemoveAt(0);
            return item;
        }

        public override void Halt()
        {
            return;
        }

        public override void Damage()
        {
            return;
        }

        public override void Update(GameTime GameTime)
        {
            if (blockState is BrokenBrickBlockState && this.Position.Y - originalLocation.Y > 200) queuedForDeletion = true; //destroy broken bricks that fall off screen


            if(bumped)                                                  // if bumped, do physics!
            {
                if (falling)                                             // logic for falling blocks
                {
                    Position = new Vector2(Position.X, Position.Y + 100 * (float)GameTime.ElapsedGameTime.TotalSeconds);
                    if (!(blockState is BrokenBrickBlockState) && Position.Y >= originalLocation.Y)     // If block goes below its original height
                    {
                        Position = new Vector2(Position.X, originalLocation.Y);
                        falling = false;
                        bumped = false;
                        blockState.Bump(mario);
                        Sprite = spriteFactory.GetCurrentSprite(Position, blockState, Sprite.isCollided);
                    }
                } else {                                                // Logic for rising blocks
                    Position = new Vector2(Position.X, Position.Y - 100 * (float)GameTime.ElapsedGameTime.TotalSeconds);
                    if (Position.Y < originalLocation.Y - 10)           // if block goes above its bump height
                    {
                        // TODO: Make it so item pops out here
                        falling = true;
                    }
                }
            }

            AABB = (new Rectangle((int)Position.X + (boundaryAdjustment / 2), (int)Position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));

            Sprite = spriteFactory.GetCurrentSprite(Position, blockState, Sprite.isCollided);
            Sprite.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, false);
            DrawAABBIfVisible(Color.Blue, spriteBatch);
        }

        public void Bump()
        {
            blockState.Bump(mario);
            SoundManager.Instance.PlaySound(SoundManager.GameSound.BUMP);
        }

        public void MoveLeft() { }
        public void MoveRight() { }
        public void Up() { }
        public void Down() { }
      
        public override void Collision(int side, GameObject Collidee)
        {
            const int TOP = 1, BOTTOM = 2, LEFT = 3, RIGHT = 4;

            if (side == BOTTOM && Collidee is Mario && Collidee.GetVelocity().Y < 0)
            {
                if (this.items != null)
                {
                    foreach (IItem item in items) System.Diagnostics.Debug.WriteLine("Item: " + item);

                }
                
                this.Bump();
               
            } else if ((side == LEFT || side == RIGHT) && Collidee is KoopaTroopa && Collidee.GetVelocity().X > 0) //Kicked koopa Troopa breaks the brick block
            {
                if (this.GetBlockState() is BrickBlockState)
                {
                    this.SetBlockState(new BrokenBrickBlockState(this));
                }
            }
        }

    }
}
