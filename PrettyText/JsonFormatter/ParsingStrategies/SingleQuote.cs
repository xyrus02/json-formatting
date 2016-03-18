namespace DL.PrettyText.JsonFormatter.ParsingStrategies
{
    internal sealed class SingleQuote : IStrategy
    {
        public char ForWhichCharacter
        {
            get { return '\''; }
        }

        public void Execute(Context context)
        {
            if (!context.IsProcessingDoubleQuoteInitiatedString && !context.WasLastCharacterABackSlash)
            {
                context.IsProcessingSingleQuoteInitiatedString = !context.IsProcessingSingleQuoteInitiatedString;
            }

            context.AppendCurrentChar();
        }
    }
}