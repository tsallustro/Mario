﻿using System;
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
        private Mario mario;

        public BrickBlockState(Block block)
        {
            this.block = block;
        }

        public void Bump()
        {
            block.SetBlockState(new BumpedBrickBlockState(block));
        }
        public void BumpBump()
        {
            block.SetBlockState(new BrokenBrickBlockState(block));
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

    public class QuestionBlock : IBlockState
    {
        private Block block;

        public QuestionBlock(Block block)
        {
            this.block = block;
        }

        public void Bump()
        {
            block.SetBlockState(new UsedBlock(block));
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
            //Do nothing
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
    public class FloorBlock : IBlockState
    {
        private Block block;

        public FloorBlock(Block block)
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
