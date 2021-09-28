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

    public class QuestionBlockState : IBlockState
    {
        private Block block;

        public QuestionBlockState(Block block)
        {
            this.block = block;
        }

        public void Bump()
        {
            block.SetBlockState(new UsedBlockState(block));
        }
    }

    public class UsedBlockState : IBlockState
    {
        private Block block;

        public UsedBlockState(Block block)
        {
            this.block = block;
        }

        public void Bump()
        {
            //Do nothing
        }
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
            block.SetBlockState(new BrokenBrickBlockState(block));
        }
    }
    public class FloorBlockState : IBlockState
    {
        private Block block;

        public FloorBlockState(Block block)
        {
            this.block = block;
        }

        public void Bump()
        {
            //Do Nothing
        }
    }
    public class StairBlockState : IBlockState
    {
        private Block block;

        public StairBlockState(Block block)
        {
            this.block = block;
        }

        public void Bump()
        {
            //Do Nothing
        }
    }
    public class HiddenBlockState : IBlockState
    {
        private Block block;

        public HiddenBlockState(Block block)
        {
            this.block = block;
        }

        public void Bump()
        {
            block.SetBlockState(new BrickBlockState(block));

            //Do some stuff on bump
        }
    }
    public class BumpedBrickBlockState : IBlockState
    {
        private Block block;

        public BumpedBrickBlockState(Block block)
        {
            this.block = block;
        }

        public void Bump()
        {
            //Do Nothing
        }
    }
    public class BrokenBrickBlockState : IBlockState
    {
        private Block block;

        public BrokenBrickBlockState(Block block)
        {
            this.block = block;
        }

        public void Bump()
        {
            //Do Nothing
        }
    }


 

}
