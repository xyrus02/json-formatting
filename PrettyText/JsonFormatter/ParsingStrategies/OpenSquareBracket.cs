namespace DL.PrettyText.JsonFormatter.ParsingStrategies
{
    internal sealed class OpenSquareBracket : IStrategy
    {
        public char ForWhichCharacter
        {
            get { return '['; }
        }

        public void Execute(Context context)
        {
            context.AppendCurrentChar();

            if (context.IsProcessingString)
            {
                return;
            }

            context.EnterArrayScope();
            context.BuildContextIndents();
        }
    }
}
