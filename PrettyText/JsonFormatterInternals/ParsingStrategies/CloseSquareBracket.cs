namespace DL.PrettyText.JsonFormatterInternals.ParsingStrategies
{
    internal sealed class CloseSquareBracket : IStrategy
    {
        public char ForWhichCharacter
        {
            get { return ']'; }
        }

        public void Execute(Context context)
        {
            if (context.IsProcessingString)
            {
                context.AppendCurrentChar();
                return;
            }

            context.CloseCurrentScope();
            context.BuildContextIndents();
            context.AppendCurrentChar();
        }
    }
}