using System;
using System.Collections.Generic;
using Raylib;
using static Raylib.Raylib;

namespace MatrixHierarchies
{
    static class Program
    {
        static void Main(string[] args)        {
            Game game = new Game();

            Random rng = new Random();

            InitWindow(640, 480, flavor());

            game.Init();

            while (!WindowShouldClose())
            {
                game.Update();
                game.Draw();
            }

            game.Shutdown();

            CloseWindow();

            string flavor()
            {
                string result = "";
                switch (rng.Next(1, 4))
                {
                    case 1:
                        result = "Are you actually reading the window name?";
                        break;
                    case 2:
                        result = "This took like a minute to do.";
                        break;
                    case 3:
                        result = "I may be wasting my time, but at least I'm amused.";
                        break;
                    default:
                        result = "Blank Text.";
                        break;
                }
                return result;
            }
        }
    }
}
