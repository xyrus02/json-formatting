namespace XyrusWorx.Json;

public class ColorJsonConfiguration
{
    public ConsoleColor CommentColor { get; set; } = ConsoleColor.Green;
    public ConsoleColor HashKeysColor { get; set; } = ConsoleColor.Magenta;
    public ConsoleColor StringsColor { get; set; } = ConsoleColor.White;
    public ConsoleColor NumbersColor { get; set; } = ConsoleColor.Yellow;
    public ConsoleColor KeywordsColor { get; set; } = ConsoleColor.Cyan;
    public ConsoleColor DelimitersColor { get; set; } = ConsoleColor.Gray;
    public ConsoleColor DefaultColor { get; set; } = ConsoleColor.DarkGray;
}