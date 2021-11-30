using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using Sound;

namespace States
{
    public class IdleBowserState : IBossState
    {
        private Bowser bowser;
        private bool left;

        public IdleBowserState(Bowser bowser, bool left)
        {
            this.bowser = bowser;
            this.left = left;
        }
        public void FaceLeft()
        {
            this.left = true;
        }
        public void FaceRight()
        {
            this.left = false;
        }
        public void Damage()
        {

        }
        
    }
    public class MovingBowserState : IBossState
    {
        private Bowser bowser;
        private bool left;

        public MovingBowserState(Bowser bowser, bool left)
        {
            this.bowser = bowser;
            this.left = left;
        }
        public void FaceLeft()
        {
            this.left = true;
        }
        public void FaceRight()
        {
            this.left = false;
        }
        public void Damage()
        {

        }

    }

    public class DamagedOneBowserState : IBossState
    {
        private Bowser bowser;
        private bool left;

        public DamagedOneBowserState(Bowser bowser, bool left)
        {
            this.bowser = bowser;
            this.left = left;
        }
        public void FaceLeft()
        {
            this.left = true;
        }
        public void FaceRight()
        {
            this.left = false;
        }
        public void Damage()
        {

        }

    }
    public class DamagedTwoBowserState : IBossState
    {
        private Bowser bowser;
        private bool left;

        public DamagedTwoBowserState(Bowser bowser, bool left)
        {
            this.bowser = bowser;
            this.left = left;
        }
        public void FaceLeft()
        {
            this.left = true;
        }
        public void FaceRight()
        {
            this.left = false;
        }
        public void Damage()
        {

        }

    }
    public class DeadBowserState : IBossState
    {
        private Bowser bowser;
        private bool left;

        public DeadBowserState(Bowser bowser, bool left)
        {
            this.bowser = bowser;
            this.left = left;
        }
        public void FaceLeft()
        {
            this.left = true;
        }
        public void FaceRight()
        {
            this.left = false;
        }
        public void Damage()
        {

        }

    }
  
  
}
