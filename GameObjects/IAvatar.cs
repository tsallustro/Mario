using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameObjects
{
    interface IAvatar : IGameObject
    {
        void MoveLeft(int pressType);
        void MoveRight(int pressType);
        void Up(int pressType);
        void Down(int pressType);
    }
}
