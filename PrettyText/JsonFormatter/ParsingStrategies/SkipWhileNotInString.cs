namespace DL.PrettyText.JsonFormatter.ParsingStrategies
{
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
