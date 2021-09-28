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

        public Block(Vector2 position, Texture2D blockSprites)
        {
            this.location = position;
            this.spriteFactory = new BlockSpriteFactory(blockSprites);
            sprite = this.spriteFactory.CreateBrickBlock(location);
            blockState = new BrickBlockState(this);
        }
        //Sets location of the block
        public void SetBlockLocation(Vector2 position)
        {
            this.sprite.location = position;
        }
        public IBlockState GetBlockState()
        {
            return this.blockState;
        }

        public void SetBlockState(IBlockState blockState)
        {
            this.blockState = blockState;
        }

        //Update all blocks
        public void Update()
        {
            sprite = spriteFactory.GetCurrentSprite(sprite.location, blockState);
            sprite.Update();
        }

        //Draw Blocks
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
