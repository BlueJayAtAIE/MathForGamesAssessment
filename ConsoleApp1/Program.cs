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

            string flavor()
            {
                string result = "";
                switch (MathFunctions.Tools.rng.Next(1, 8))
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
                        result = "Programming: change two numbers to do something completely different.";
                        break;
                    case 5:
                        result = "Raylib: Now with more Vectors!";
                        break;
                    case 6:
                        result = "Collisions everywhere!";
                        break;
                    case 7:
                        result = "So much content!";
                        break;
                    default:
                        result = "Tanks for N o t h i n g.";
                        break;
                }
                return result;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;

            InitWindow(640, 480, flavor());
            SetTargetFPS(60);

            game.Init();

            while (!WindowShouldClose() && game.remainingTime > 0)
            {
                game.Update();
                game.Draw();
            }

            game.Shutdown();

            CloseWindow();
        }
    }
}
