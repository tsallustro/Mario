using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GameObjects;

namespace States
{
    public class QuestionBlockState : IBlockState
    {
        private IBlock block;

        public QuestionBlockState(IBlock block)
        {
            this.block = block;
            this.block.SetFalling(false);
            this.block.SetBumped(false);
        }

        public void Bump(Mario Mario)
        {
            block.SetBlockState(new BumpedQuestionBlockState(block));
        }
    }
    public class BumpedQuestionBlockState : IBlockState
    {
        private IBlock block;

        public BumpedQuestionBlockState(IBlock block)
        {
            this.block = block;
            this.block.SetFalling(false);
            this.block.SetBumped(true);
        }

        public void Bump(Mario Mario)
        {
            // Used to reset to BrickBlockState
            block.SetBlockState(new UsedBlockState(block));
        }
    }

    public class UsedBlockState : IBlockState
    {
        private IBlock block;

        public UsedBlockState(IBlock block)
        {
            this.block = block;
            this.block.SetFalling(false);
            this.block.SetBumped(false);
        }

        public void Bump(Mario Mario)
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
            this.block.SetFalling(false);
            this.block.SetBumped(false);
        }

        public void Bump(Mario mario) // DO CHECK FOR MARIO STATE
        {
            // if mario state != normal, then make broken brick
            // else make brick
            if (mario.CanBreakBricks())
            {
                block.SetBlockState(new BrokenBrickBlockState(block));
            } else
            {
                block.SetBlockState(new BumpedBrickBlockState(block));
            }
        }
    }
    public class FloorBlockState : IBlockState
    {
        private IBlock block;

        public FloorBlockState(IBlock block)
        {
            this.block = block;
            this.block.SetFalling(false);
            this.block.SetBumped(false);
        }

        public void Bump(Mario Mario)
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
            this.block.SetFalling(false);
            this.block.SetBumped(false);
        }

        public void Bump(Mario Mario)
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
            this.block.SetFalling(false);
            this.block.SetBumped(false);
        }

        public void Bump(Mario Mario)
        {
            block.SetBlockState(new BumpedBrickBlockState(block));
        }
    }
    public class BumpedBrickBlockState : IBlockState
    {
        private IBlock block;

        public BumpedBrickBlockState(IBlock block)
        {
            this.block = block;
            this.block.SetFalling(false);
            this.block.SetBumped(true);
        }

        public void Bump(Mario Mario)
        {
            // Used to reset to BrickBlockState
            block.SetBlockState(new BrickBlockState(block));
        }
    }
    public class BrokenBrickBlockState : IBlockState
    {
        private IBlock block;

        public BrokenBrickBlockState(IBlock block)
        {
            this.block = block;
            this.block.SetFalling(true);
            this.block.SetBumped(true);
        }

        public void Bump(Mario Mario)
        {
            //Delete the GameObject & Sprite
        }
    }


 

}
