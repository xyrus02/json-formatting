# DL.PrettyText

C# utils to 
* Format JSON with colors
* Change system console colors
* Print colored text to the system console.

## Nuget package 

Package available at [nuget.org/packages/DL.PrettyText.NET](https://www.nuget.org/packages/DL.PrettyText.NET)

## Screenshots

![Example on console with black background](https://raw.githubusercontent.com/dluc/DL.PrettyText.NET/master/demo_black_bg.png "Example on console with black background")

![Example on console with white background](https://raw.githubusercontent.com/dluc/DL.PrettyText.NET/master/demo_white_bg.png "Example on console with white background")

## API

Serialize an object to JSON and format the JSON with indentation:

`string text = JsonFormatter.Format(object obj, ushort indentation = 4)`

Format a JSON string with indentation:

`string text = JsonFormatter.Format(string json, ushort indent = 4)`

Minify a JSON string:

`string text = JsonFormatter.Minify(string json)`

Print an object to console using JSON notation and colors, either on black or white background:

`Console.PrintJson(object obj, ushort indentation = 4, bool blackBg = true)`

Print a JSON string to console using colors, either on black or white background:

`Console.PrintJson(string json, ushort indentation = 4, bool blackBg = true)`

Change the default foreground (Gray) and background (Black) console colors to new values:

`ConsoleColors.ChangeScreenColors(System.Drawing.Color foreground, System.Drawing.Color background)`

Change one of the 16 console colors, to match one of the many colors available in System.Drawing:

`ConsoleColors.RedefineColor(System.ConsoleColor consoleColor, System.Drawing.Color targetColor)`

Change one of the 16 console colors, to new RGB values:

`ConsoleColors.RedefineColor(System.ConsoleColor consoleColor, uint red, uint green, uint blue)`
