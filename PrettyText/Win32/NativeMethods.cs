namespace DL.PrettyText.Win32
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    internal class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref ConsoleScreenBufferInfoEx csbe);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref ConsoleScreenBufferInfoEx csbe);

        [StructLayout(LayoutKind.Sequential)]
        internal struct ConsoleScreenBufferInfoEx
        {
            internal int cbSize;
            internal Coord dwSize;
            internal Coord dwCursorPosition;
            internal ushort wAttributes;
            internal SmallRect srWindow;
            internal Coord dwMaximumWindowSize;
            internal ushort wPopupAttributes;
            internal bool bFullscreenSupported;
            internal ColorRef black;
            internal ColorRef darkBlue;
            internal ColorRef darkGreen;
            internal ColorRef darkCyan;
            internal ColorRef darkRed;
            internal ColorRef darkMagenta;
            internal ColorRef darkYellow;
            internal ColorRef gray;
            internal ColorRef darkGray;
            internal ColorRef blue;
            internal ColorRef green;
            internal ColorRef cyan;
            internal ColorRef red;
            internal ColorRef magenta;
            internal ColorRef yellow;
            internal ColorRef white;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Coord
        {
            internal short X;
            internal short Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SmallRect
        {
            internal short Left;
            internal short Top;
            internal short Right;
            internal short Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct ColorRef
        {
            internal uint ColorDWORD;

            internal ColorRef(Color color)
            {
                this.ColorDWORD = color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
            }

            internal ColorRef(uint r, uint g, uint b)
            {
                this.ColorDWORD = r + (g << 8) + (b << 16);
            }

            internal Color GetColor()
            {
                return Color.FromArgb((int)(0x000000FFU & this.ColorDWORD), (int)(0x0000FF00U & this.ColorDWORD) >> 8, (int)(0x00FF0000U & this.ColorDWORD) >> 16);
            }

            internal void SetColor(Color color)
            {
                this.ColorDWORD = color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
            }
        }
    }
}
