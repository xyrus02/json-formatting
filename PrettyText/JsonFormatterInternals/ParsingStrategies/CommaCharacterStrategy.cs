namespace DL.PrettyText.JsonFormatterInternals.ParsingStrategies
{
    internal sealed class Comma : IStrategy
    {
        public char ForWhichCharacter
        {
            get { return ','; }
        }

        public void Execute(Context context)
        {
            context.AppendCurrentChar();

            if (context.IsProcessingString)
            {
                return;
            }

            context.BuildContextIndents();
            context.IsProcessingVariableAssignment = false;
        }
    }
}
