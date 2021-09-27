using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;

namespace States
{
    public interface IBlockState
    {
        public void Bump();
    }

    public class BrickBlockState : IBlockState
    {
        private Block block;

        public BrickBlockState(Block block)
        {
            this.block = block;
        }

        public void Bump()
        {
            block.SetBlockState(new BumpBlock(block));
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
            block.SetBlockState(new UsedBlock(block));

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
            block.SetBlockState(new BrickBlockState(block));

            //Do some stuff on bump
        }
    }
}
