﻿using System;

namespace Game1
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Sprint0())
                game.Run();
        }
    }
}