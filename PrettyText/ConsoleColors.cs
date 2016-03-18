namespace DL.PrettyText
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using DL.PrettyText.Win32;

    public static class ConsoleColors
    {
        private const int StdOutputHandle = -11;
        private static readonly IntPtr InvalidHandleValue = new IntPtr(-1);

        public static int RedefineScreenColors(Color foreground, Color background)
        {
            var irc = RedefineColor(ConsoleColor.Gray, foreground);
            return irc != 0 ? irc : RedefineColor(ConsoleColor.Black, background);
        }

        public static int RedefineColor(ConsoleColor consoleColor, Color targetColor)
        {
            return RedefineColor(consoleColor, targetColor.R, targetColor.G, targetColor.B);
        }

        public static int RedefineColor(ConsoleColor consoleColor, uint red, uint green, uint blue)
        {
            var consoleScreenBufferInfoEx = new NativeMethods.ConsoleScreenBufferInfoEx();
            consoleScreenBufferInfoEx.cbSize = Marshal.SizeOf(consoleScreenBufferInfoEx);
            var consoleOutput = NativeMethods.GetStdHandle(StdOutputHandle);
            if (consoleOutput == InvalidHandleValue)
            {
                return Marshal.GetLastWin32Error();
            }

            var screenBufferInfoEx = NativeMethods.GetConsoleScreenBufferInfoEx(consoleOutput, ref consoleScreenBufferInfoEx);
            if (!screenBufferInfoEx)
            {
                return Marshal.GetLastWin32Error();
            }

            switch (consoleColor)
            {
                case ConsoleColor.Black:
                    consoleScreenBufferInfoEx.black = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.DarkBlue:
                    consoleScreenBufferInfoEx.darkBlue = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.DarkGreen:
                    consoleScreenBufferInfoEx.darkGreen = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.DarkCyan:
                    consoleScreenBufferInfoEx.darkCyan = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.DarkRed:
                    consoleScreenBufferInfoEx.darkRed = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.DarkMagenta:
                    consoleScreenBufferInfoEx.darkMagenta = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.DarkYellow:
                    consoleScreenBufferInfoEx.darkYellow = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.Gray:
                    consoleScreenBufferInfoEx.gray = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.DarkGray:
                    consoleScreenBufferInfoEx.darkGray = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.Blue:
                    consoleScreenBufferInfoEx.blue = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.Green:
                    consoleScreenBufferInfoEx.green = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.Cyan:
                    consoleScreenBufferInfoEx.cyan = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.Red:
                    consoleScreenBufferInfoEx.red = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.Magenta:
                    consoleScreenBufferInfoEx.magenta = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.Yellow:
                    consoleScreenBufferInfoEx.yellow = new NativeMethods.ColorRef(red, green, blue);
                    break;
                case ConsoleColor.White:
                    consoleScreenBufferInfoEx.white = new NativeMethods.ColorRef(red, green, blue);
                    break;
            }

            ++consoleScreenBufferInfoEx.srWindow.Bottom;
            ++consoleScreenBufferInfoEx.srWindow.Right;
            screenBufferInfoEx = NativeMethods.SetConsoleScreenBufferInfoEx(consoleOutput, ref consoleScreenBufferInfoEx);
            return !screenBufferInfoEx ? Marshal.GetLastWin32Error() : 0;
        }
    }
}