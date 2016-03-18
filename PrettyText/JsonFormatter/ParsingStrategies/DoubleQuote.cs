namespace DL.PrettyText.JsonFormatter.ParsingStrategies
{
    internal sealed class DoubleQuote : IStrategy
    {
        public char ForWhichCharacter
        {
            get { return '"'; }
        }

        public void Execute(Context context)
        {
            if (!context.IsProcessingSingleQuoteInitiatedString && !context.WasLastCharacterABackSlash)
            {
                context.IsProcessingDoubleQuoteInitiatedString = !context.IsProcessingDoubleQuoteInitiatedString;
            }

            context.AppendCurrentChar();
        }
    }
}
