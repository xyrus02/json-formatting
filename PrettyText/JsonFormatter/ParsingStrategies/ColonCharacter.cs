namespace DL.PrettyText.JsonFormatter.ParsingStrategies
{
    internal sealed class ColonCharacter : IStrategy
    {
        public char ForWhichCharacter
        {
            get { return ':'; }
        }

        public void Execute(Context context)
        {
            if (context.IsProcessingString)
            {
                context.AppendCurrentChar();
                return;
            }

            context.IsProcessingVariableAssignment = true;
            context.AppendCurrentChar();
            context.AppendSpace();
        }
    }
}
