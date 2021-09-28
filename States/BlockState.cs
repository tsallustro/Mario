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
        private IBlock block;

        public QuestionBlockState(IBlock block)
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
        private IBlock block;

        public UsedBlockState(IBlock block)
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
        private IBlock block;

        public BrickBlockState(IBlock block)
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
        private IBlock block;

        public FloorBlockState(IBlock block)
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
        private IBlock block;

        public StairBlockState(IBlock block)
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
        private IBlock block;

        public HiddenBlockState(IBlock block)
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
        private IBlock block;

        public BumpedBrickBlockState(IBlock block)
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
        private IBlock block;

        public BrokenBrickBlockState(IBlock block)
        {
            this.block = block;
        }

        public void Bump()
        {
            //Do Nothing
        }
    }


 

}
