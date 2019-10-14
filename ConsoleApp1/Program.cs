using System;
using System.Collections.Generic;
using Raylib;
using static Raylib.Raylib;

namespace MatrixHierarchies
{
    static class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            Random rng = new Random();
            string flavor()
            {
                string result = "";
                switch (rng.Next(1, 6))
                {
                    case 1:
                        result = "Are you actually reading the window name?";
                        break;
                    case 2:
                        result = "This seems a little silly to do.";
                        break;
                    case 3:
                        result = "I may be wasting my time, but at least I'm amused.";
                        break;
                    case 4:
                        result = "Change two numbers to win.";
                        break;
                    case 5:
                        result = "Raylib: Now with more Vectors!";
                        break;
                    default:
                        result = "Tanks for N o t h i n g";
                        break;
                }
                return result;
            }

            InitWindow(640, 480, flavor());

            game.Init();

            while (!WindowShouldClose())
            {
                game.Update();
                game.Draw();
            }

            game.Shutdown();

            CloseWindow();
        }
    }
}
