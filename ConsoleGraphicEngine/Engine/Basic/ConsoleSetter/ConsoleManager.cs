using System;
using System.Runtime.InteropServices;

namespace ConsoleGraphicEngine3D.Engine.Basic.ConsoleSetter
{
    public static class ConsoleManager
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]

        private static extern IntPtr GetConsoleWindow();
        private static readonly IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int _HIDE = 0;
        private const int _MAXIMIZE = 3;
        private const int _MINIMIZE = 6;
        private const int _RESTORE = 9;

        public static void MaximizeConsole()
        {
            ShowWindow(ThisConsole, _MAXIMIZE);

            Console.BufferHeight = short.MaxValue - 1;
            Console.BufferWidth = Console.WindowWidth;
        }

        public static void MinimizeConsole()
        {
            ShowWindow(ThisConsole, _MINIMIZE);

            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
        }

        public static void HideConsole()
        {
            ShowWindow(ThisConsole, _HIDE);
        }

        public static void RestoreConsole()
        {
            ShowWindow(ThisConsole, _RESTORE);
        }

        //TODO: 
        //public Vector2Int GetCharSize()
        //{
        //    Console.WriteLine(Sc);
        //}
    }
}
