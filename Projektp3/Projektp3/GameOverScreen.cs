using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektp3
{
    public class GameOverScreen
    {
        private static readonly string[] GameOverFrames =
        {
        @"  _____          __  __ ______    ______      ________ _____  ",
        @" / ____|   /\   |  \/  |  ____|  / __ \ \    / /  ____|  __ \ ",
        @"| |  __   /  \  | \  / | |__    | |  | \ \  / /| |__  | |__) |",
        @"| | |_ | / /\ \ | |\/| |  __|   | |  | |\ \/ / |  __| |  _  / ",
        @"| |__| |/ ____ \| |  | | |____  | |__| | \  /  | |____| | \ \ ",
        @" \_____/_/    \_\_|  |_|______|  \____/   \/   |______|_|  \_\"
    };

        private static readonly string[] WinFrames =
        {
        @"__     __          __          ___       _ ",
        @"\ \   / /          \ \        / (_)     | |",
        @" \ \_/ /__  _   _   \ \  /\  / / _ _ __ | |",
        @"  \   / _ \| | | |   \ \/  \/ / | | '_ \| |",
        @"   | | (_) | |_| |    \  /\  /  | | | | |_|",
        @"   |_|\___/ \__,_|     \/  \/   |_|_| |_(_)"
    };

        public static void Show(bool victory)
        {
            Console.Clear();
            Console.CursorVisible = false;


            AnimateTitle(victory);

            ShowAnimatedOptions();
        }

        private static void AnimateTitle(bool victory)
        {
            string[] frames = victory ? WinFrames : GameOverFrames;

            for (int i = 0; i < frames.Length; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - frames[i].Length / 2, 5 + i);

                foreach (char c in frames[i])
                {
                    Console.Write(c);
                    Thread.Sleep(10);
                }

                Console.WriteLine();
            }

            for (int i = 0; i < 3; i++)
            {
                Console.ForegroundColor = victory ? ConsoleColor.Green : ConsoleColor.Red;
                Thread.Sleep(300);

                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(300);
            }
        }

        

        private static void ShowAnimatedOptions()
        {
            string[] options =
            {
            "┌─────────────────────────────────┐",
            "│          GAME OVER              │",
            "├─────────────────────────────────┤",
            "│    [R] RESTART GAME             │",
            "│    [Q] QUIT                     │",
            "└─────────────────────────────────┘"
        };

            for (int i = 0; i < options.Length; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - options[i].Length / 2, 16 + i);

                foreach (char c in options[i])
                {
                    Console.Write(c);
                    Thread.Sleep(5);
                }
            }

            Console.CursorVisible = true;
            Console.SetCursorPosition(Console.WindowWidth / 2 - 4, 22);

            while (true)
            {
                Console.Write("> ");
                var key = Console.ReadKey(true).Key;

                Console.SetCursorPosition(Console.WindowWidth / 2 - 4, 22);
                Console.Write("  "); 

                switch (key)
                {
                    case ConsoleKey.R:
                        FlashOption("RESTART GAME");
                        GameLoop.GameStart();
                        return;
                    case ConsoleKey.Q:
                        FlashOption("QUIT");
                        Environment.Exit(0);
                        return;
                }
            }
        }

        private static void FlashOption(string option)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(Console.WindowWidth / 2 - option.Length / 2, 22);
                Console.Write(option);
                Thread.Sleep(100);

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(Console.WindowWidth / 2 - option.Length / 2, 22);
                Console.Write(option);
                Thread.Sleep(100);
            }
        }
    }
}
