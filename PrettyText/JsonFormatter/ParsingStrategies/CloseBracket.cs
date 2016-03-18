namespace DL.PrettyText.JsonFormatter.ParsingStrategies
{
    internal sealed class CloseBracket : IStrategy
    {
        public char ForWhichCharacter
        {
            get { return '}'; }
        }

        public void Execute(Context context)
        {
            if (context.IsProcessingString)
            {
                context.AppendCurrentChar();
                return;
            }

            PeformNonStringPrint(context);
        }

        private static void PeformNonStringPrint(Context context)
        {
            context.CloseCurrentScope();
            context.BuildContextIndents();
            context.AppendCurrentChar();
        }
    }
}
