using ConsoleGraphicEngine.Engine.Basic.Tools;
using System;
using System.Runtime.InteropServices;

namespace ConsoleGraphicEngine.Engine.Basic.ConsoleSetter
{
    internal static class ConsoleManager
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]

        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int _HIDE = 0;
        private const int _MAXIMIZE = 3;
        private const int _MINIMIZE = 6;
        private const int _RESTORE = 9;

        public static void MaximizeConsole()
        {
            ShowWindow(ThisConsole, _MAXIMIZE);

            Vector2Int consoleSize = new Vector2Int(Console.WindowWidth, Console.WindowHeight);

            Console.BufferHeight = short.MaxValue - 1;
            Console.BufferWidth = consoleSize.X;
        }

        public static void MinimizeConsole()
        {
            ShowWindow(ThisConsole, _MINIMIZE);
        }

        public static void HideConsole()
        {
            ShowWindow(ThisConsole, _HIDE);
        }

        public static void RestoreConsole()
        {
            ShowWindow(ThisConsole, _RESTORE);
        }
    }
}
