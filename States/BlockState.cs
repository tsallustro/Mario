using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
    public interface IBlockState
    {
        public void Bump();
    }

    public class Block
    {
        public IBlockState state;

        public Block()
        {
            state = new NormalBlock(this);
        }
    }

    public class NormalBlock : IBlockState
    {
        private Block block;

        public NormalBlock(Block block)
        {
            this.block = block;
        }

        public void Bump()
        {
            block.state = new BumpBlock(block);

            //Do some stuff on bump
        }
    }

    public class BumpBlock : IBlockState
    {
        private Block block;

        public BumpBlock(Block block)
        {
            this.block = block;
        }

        public void Bump()
        {
            block.state = new UsedBlock(block);

            //Do some stuff on used
        }
    }

    public class UsedBlock : IBlockState
    {
        private Block block;

        public UsedBlock(Block block)
        {
            this.block = block;
        }

        public void Bump()
        {
            //Do some stuff on bump based on used block
        }
    }

    public class HiddenBlock : IBlockState
    {
        private Block block;

        public HiddenBlock(Block block)
        {
            this.block = block;
        }

        public void Bump()
        {
            block.state = new NormalBlock(block);

            //Do some stuff on bump
        }
    }
}
