using System;

namespace Commands
{
    /// <summary>
    /// Console colors for every type of text
    /// </summary>
    public struct ConsoleColorSet
    {
        public ConsoleColor BackgroundColor { get; }
        public ConsoleColor CommandColor { get; }
        public ConsoleColor TextColor { get; }

        public ConsoleColorSet(ConsoleColor backgroundColor, ConsoleColor commandColor, ConsoleColor textColor)
        {
            BackgroundColor = backgroundColor;
            CommandColor = commandColor;
            TextColor = textColor;
        }

        public static ConsoleColorSet BlackGreen => new ConsoleColorSet(ConsoleColor.Black, ConsoleColor.Green, ConsoleColor.White);
        public static ConsoleColorSet BlackBlue => new ConsoleColorSet(ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.White);
        public static ConsoleColorSet WhiteGreen => new ConsoleColorSet(ConsoleColor.White, ConsoleColor.Green, ConsoleColor.Black);
        public static ConsoleColorSet WhiteBlue => new ConsoleColorSet(ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Black);
    }
}
