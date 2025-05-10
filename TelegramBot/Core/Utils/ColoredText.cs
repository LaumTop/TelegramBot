using System;

namespace TelegramBot.Core.Utils
{
    static class ColoredText
    {
        public static void SetConsoleColorAndWriteLine(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void SetConsoleColorAndWrite(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }
    }
}
