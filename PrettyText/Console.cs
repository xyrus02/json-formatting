using System.Text.Json;
using System.Text.Json.Serialization;

namespace DL.PrettyText
{
    using System;
    using System.Drawing;
    using System.Text.RegularExpressions;

    public static class Console
    {
        private const ConsoleColor HashKeysColor = ConsoleColor.DarkYellow;
        private const ConsoleColor StringsColor = ConsoleColor.DarkGray;
        private const ConsoleColor NumbersColor = ConsoleColor.DarkGreen;
        private const ConsoleColor BoolColor = ConsoleColor.DarkMagenta;
        private const ConsoleColor NullColor = ConsoleColor.DarkBlue;
        private const ConsoleColor ArraysColor = ConsoleColor.DarkCyan;
        private const ConsoleColor HashesColor = ConsoleColor.DarkRed;
        private const string Escape = "◪";
        
        public static void PrintJson(object obj, ushort indentation = 4, bool blackBg = true)
        {
            InitColors(blackBg);

            var json = JsonSerializer.Serialize(obj);
            json = new JsonFormatterInternals.JsonFormatter(indentation).Format(json);

            if (json.Contains(Escape))
            {
                WriteToConsole(json);
            }

            WriteToConsole(ColorizeJson(json));
        }

        private static void InitColors(bool blackBg = true)
        {
            if (blackBg)
            {
                var screenTextColor = Color.LightGray;
                Color screenBackgroundColor = Color.Black;
                ConsoleColors.ChangeScreenColors(screenTextColor, screenBackgroundColor);

                ConsoleColors.RedefineColor(HashKeysColor, Color.DeepSkyBlue);
                ConsoleColors.RedefineColor(StringsColor, Color.LightGreen);
                ConsoleColors.RedefineColor(NumbersColor, Color.Orange);
                ConsoleColors.RedefineColor(BoolColor, Color.Yellow);
                ConsoleColors.RedefineColor(NullColor, Color.White);
                ConsoleColors.RedefineColor(ArraysColor, Color.White);
                ConsoleColors.RedefineColor(HashesColor, Color.DarkGray);
            }
            else
            {
                var screenTextColor = Color.DimGray;
                var screenBackgroundColor = Color.White;
                ConsoleColors.ChangeScreenColors(screenTextColor, screenBackgroundColor);

                ConsoleColors.RedefineColor(HashKeysColor, Color.DarkBlue);
                ConsoleColors.RedefineColor(StringsColor, Color.DarkRed);
                ConsoleColors.RedefineColor(NumbersColor, Color.OrangeRed);
                ConsoleColors.RedefineColor(BoolColor, Color.Blue);
                ConsoleColors.RedefineColor(NullColor, Color.DimGray);
                ConsoleColors.RedefineColor(ArraysColor, Color.Black);
                ConsoleColors.RedefineColor(HashesColor, Color.Black);
            }
        }

        private static void WriteToConsole(string text)
        {
            var words = text.Split(new[] { Escape }, StringSplitOptions.None);
            foreach (var word in words)
            {
                if (string.IsNullOrEmpty(word))
                {
                    continue;
                }

                switch (word[0])
                {
                    case 'A':
                        System.Console.ForegroundColor = ArraysColor;
                        break;
                    case 'B':
                        System.Console.ForegroundColor = BoolColor;
                        break;
                    case 'D':
                        System.Console.ForegroundColor = NumbersColor;
                        break;
                    case 'H':
                        System.Console.ForegroundColor = HashesColor;
                        break;
                    case 'K':
                        System.Console.ForegroundColor = HashKeysColor;
                        break;
                    case 'N':
                        System.Console.ForegroundColor = NullColor;
                        break;
                    case 'S':
                        System.Console.ForegroundColor = StringsColor;
                        break;
                }

                System.Console.Write(word.Substring(1));
                System.Console.ResetColor();
            }
        }

        private static string ColorizeJson(string text)
        {
            text = ColorizeArrays(text);
            text = ColorizeStrings(text);
            text = ColorizeNumbers(text);
            text = ColorizeBooleans(text);
            text = ColorizeCurlyBraces(text);
            return ColorizeNulls(text);
        }

        private static string ColorizeArrays(string text)
        {
            return Regex.Replace(text, "(\\s*)(\\[\\]|\\[|\\])(,?\\s*(:?\n|$))", "$1" + Escape + "A$2" + Escape + "X$3");
        }

        private static string ColorizeStrings(string text)
        {
            text = Regex.Replace(text, "(\")([^\"\\\\]*(?:\\\\.[^\"\\\\]*)*?)(\"):", "$1" + Escape + "K$2" + Escape + "X$3:");
            return Regex.Replace(text, "(\")([^\"\\\\]*(?:\\\\.[^\"\\\\]*)*?)(\")", "$1" + Escape + "S$2" + Escape + "X$3");
        }

        private static string ColorizeNumbers(string text)
        {
            return Regex.Replace(text, "(-?\\d+\\.?\\d*)(,?\\s*\n)", Escape + "D$1" + Escape + "X$2");
        }

        private static string ColorizeBooleans(string text)
        {
            return Regex.Replace(text, "(true|false)(,?\\s*\n)", Escape + "B$1" + Escape + "X$2");
        }

        private static string ColorizeCurlyBraces(string text)
        {
            return Regex.Replace(text, "(\\s*)({}|{|})(,?\\s*(?:\n|$))", "$1" + Escape + "H$2" + Escape + "X$3");
        }

        private static string ColorizeNulls(string text)
        {
            return Regex.Replace(text, "null(,?\\s*\n)", Escape + "Nnull" + Escape + "X$1");
        }
    }
}