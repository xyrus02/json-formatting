namespace DL.PrettyText.JsonFormatterInternals.ParsingStrategies
{
    using DL.PrettyText.JsonFormatterInternals;

    internal sealed class SkipWhileNotInString : IStrategy
    {
        public SkipWhileNotInString(char selectionCharacter)
        {
            this.ForWhichCharacter = selectionCharacter;
        }

        public char ForWhichCharacter { get; }

        public void Execute(Context context)
        {
            if (context.IsProcessingString)
            {
                context.AppendCurrentChar();
            }
        }
    }
}
