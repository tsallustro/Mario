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
        private Mario mario;
        private BlockSpriteFactory spriteFactory;
        private Vector2 location;
        private Vector2 originalLocation;
        private Boolean falling = false;
        private Boolean bumped = false;
        private HashSet<IItem> items; // Future use for storing items in block

        public Block(Vector2 position, Texture2D blockSprites, Mario Mario)
        {
            location = position;
            originalLocation = position;
            mario = Mario;
            spriteFactory = new BlockSpriteFactory(blockSprites);
            blockState = new BrickBlockState(this);
            sprite = spriteFactory.CreateBrickBlock(location);
        }

        // Future constructor for adding items to block
        public Block(Vector2 position, Texture2D blockSprites, Mario Mario, HashSet<IItem> items)
        {
            location = position;
            originalLocation = position;
            mario = Mario;
            spriteFactory = new BlockSpriteFactory(blockSprites);
            this.items = items;
            blockState = new BrickBlockState(this);
            sprite = spriteFactory.CreateBrickBlock(location);
        }

        public void SetLocation(Vector2 position)
        {
            sprite.location = position;
        }
        public Vector2 GetLocation()
        {
            return location;
        }
        public void SetFalling(Boolean Falling)
        {
            this.falling = Falling;
        }
        public void SetBumped(Boolean bumped)
        {
            this.bumped = bumped;
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

            /*
            if (bumped && !(blockState is UsedBlockState))
            {
                if (!falling && location.Y >= originalLocation.Y - 10)                                // block is less than desired height
                {
                    location.Y = location.Y - 100 * (float)GameTime.ElapsedGameTime.TotalSeconds;
                } else if (!falling && location.Y < originalLocation.Y - 10)                        // block hits its desired height.
                {
                    falling = true;
                } else if (falling && location.Y < originalLocation.Y)                              // block is falling and heigher than original location
                {
                    location.Y = location.Y + 100 * (float)GameTime.ElapsedGameTime.TotalSeconds;
                }
                else {                                                                         // if block is falling and below original location (reset)
                    location.Y = originalLocation.Y;
                    if (blockState is BumpedQuestionBlockState)
                    {
                        blockState = new UsedBlockState(this);
                    }
                    else if (!(mario.GetPowerState() is StandardMario))
                    {
                        blockState = new BrokenBrickBlockState(this);
                    }
                    bumped = false;
                    falling = false;
                }
            }

            if (blockState is BrokenBrickBlockState)
            {
                location.Y = location.Y + 300 * (float)GameTime.ElapsedGameTime.TotalSeconds;

            }
            */

            if(bumped)                                                  // if bumped, do physics!
            {
                if (falling)                                             // logic for falling blocks
                {
                    location.Y = location.Y + 100 * (float)GameTime.ElapsedGameTime.TotalSeconds;
                    if (!(blockState is BrokenBrickBlockState) && location.Y >= originalLocation.Y)     // If block goes below its original height
                    {
                        location.Y = originalLocation.Y;
                        falling = false;
                        bumped = false;
                        blockState.Bump(mario);
                        sprite = spriteFactory.GetCurrentSprite(location, blockState);
                    }
                } else {                                                // Logic for rising blocks
                    location.Y = location.Y - 100 * (float)GameTime.ElapsedGameTime.TotalSeconds;
                    if (location.Y < originalLocation.Y - 10)           // if block goes above its bump height
                    {
                        // TODO: Make it so item pops out here
                        falling = true;
                    }
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
            blockState.Bump(mario);
            //sprite = spriteFactory.CreateBumpedBrickBlock(location);
        }
    }
}
