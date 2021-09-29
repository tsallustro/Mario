using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;
using Factories;

namespace GameObjects
{
    public class Block : IBlock
    {
        private ISprite sprite;
        private IBlockState blockState;
        private BlockSpriteFactory spriteFactory;
        private Vector2 location;
        private Vector2 originalLocation;
        private Boolean falling = false;
        private Boolean bumped = false;
        private HashSet<IItem> items; // Future use for storing items in block

        public Block(Vector2 position, Texture2D blockSprites)
        {
            location = position;
            originalLocation = position;
            spriteFactory = new BlockSpriteFactory(blockSprites);
            sprite = spriteFactory.CreateBrickBlock(location);
            blockState = new BrickBlockState(this);
        }

        // Future constructor for adding items to block
        public Block(Vector2 position, Texture2D blockSprites, HashSet<IItem> items)
        {
            location = position;
            originalLocation = position;
            spriteFactory = new BlockSpriteFactory(blockSprites);
            this.items = items;
            sprite = spriteFactory.CreateBrickBlock(location);
            blockState = new BrickBlockState(this);
        }

        public void SetLocation(Vector2 position)
        {
            sprite.location = position;
        }
        public Vector2 GetLocation()
        {
            return location;
        }

        public IBlockState GetBlockState()
        {
            return blockState;
        }
   

        public void SetBlockState(IBlockState blockState)
        {
            this.blockState = blockState;
        }

        public void Update(GameTime GameTime)
        {
            //System.Diagnostics.Debug.WriteLine("IM HERE:" + blockState);

            if (bumped)
            {
                System.Diagnostics.Debug.WriteLine("IM HERE:" + (originalLocation.Y - 25));
                if (!falling && location.Y >= originalLocation.Y-25)                                // block is less than desired height
                {
                    location.Y = location.Y - 25 * (float)GameTime.ElapsedGameTime.TotalSeconds;
                } else if (!falling && location.Y < originalLocation.Y - 25)                        // block hits its desired height.
                {
                    falling = true;
                } else if (falling && location.Y < originalLocation.Y)                              // block is falling and heigher than original location
                {
                    location.Y = location.Y + 25 * (float)GameTime.ElapsedGameTime.TotalSeconds;
                }
                else {                                                                              // if block is falling and below original location (reset)
                    location.Y = originalLocation.Y;
                    if (blockState is BumpedQuestionBlockState)
                    {
                        sprite = spriteFactory.CreateUsedBlock(location);
                    } else
                    {
                        sprite = spriteFactory.CreateBrickBlock(location);
                    }
                    bumped = false;
                    falling = false;
                }

            }
            
            sprite = spriteFactory.GetCurrentSprite(location, blockState);
            sprite.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, false);
        }

        public void Bump()
        {
            bumped = true;
            falling = false;
            blockState.Bump();
            //sprite = spriteFactory.CreateBumpedBrickBlock(location);

        }

    }
}
