using System;
using System.Collections.Generic;
using System.Text;

namespace Game1
{
    abstract class MarioState
    {
        protected AvatarSprite Sprite { get; set; }
        public abstract void MoveLeft();
        public abstract void MoveRight();
        public abstract void Jump();
        public abstract void Crouch();
        public abstract void DashOrThrowFireball(); // Future command
    }
}
