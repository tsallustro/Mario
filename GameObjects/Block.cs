using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;
using Factories;
using Collisions;

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
        private HashSet<IItem> items;

        public Block(Vector2 position, Texture2D blockSprites, Mario Mario)
            : base(position, new Vector2(0, 0), new Vector2(0, 0))
        {
            originalLocation = position;
            mario = Mario;
            spriteFactory = new BlockSpriteFactory(blockSprites);
            blockState = new BrickBlockState(this);
            Sprite = spriteFactory.CreateBrickBlock(position, false);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
        }

        // Future constructor for adding items to block
        public Block(Vector2 position, Texture2D blockSprites, Mario Mario, HashSet<IItem> items)
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
            //Sprite = spriteFactory.CreateBumpedBrickBlock(location);
        }

        public void MoveLeft() { }
        public void MoveRight() { }
        public void Up() { }
        public void Down() { }
      
        public override void Collision(int side, GameObject Collidee)
        {
            
            if (side == CollisionHandler.BOTTOM && Collidee is Mario && Collidee.GetVelocity().Y >0)
            {
                System.Diagnostics.Debug.WriteLine("Collided! Bumping...");
                this.Bump();
               
            }
        }
    }
}
