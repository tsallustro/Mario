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
        private HashSet<IItem> items; // Future use for storing items in block

        public Block(Vector2 position, Texture2D blockSprites)
        {
            location = position;
            spriteFactory = new BlockSpriteFactory(blockSprites);
            sprite = spriteFactory.CreateBrickBlock(location);
            blockState = new BrickBlockState(this);
        }

        // Future constructor for adding items to block
        public Block(Vector2 position, Texture2D blockSprites, HashSet<IItem> items)
        {
            location = position;
            spriteFactory = new BlockSpriteFactory(blockSprites);
            this.items = items;
            sprite = spriteFactory.CreateBrickBlock(location);
            blockState = new BrickBlockState(this);
        }

        public void SetBlockLocation(Vector2 position)
        {
            sprite.location = position;
        }

        public IBlockState GetBlockState()
        {
            return blockState;
        }

        public void SetBlockState(IBlockState blockState)
        {
            this.blockState = blockState;
        }

        public void Update()
        {
            sprite = spriteFactory.GetCurrentSprite(sprite.location, blockState);
            sprite.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, false);
        }

        public void Bump()
        {
            blockState.Bump();
        }
    }
}
