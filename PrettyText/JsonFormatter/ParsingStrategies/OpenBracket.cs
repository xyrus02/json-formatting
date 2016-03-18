namespace DL.PrettyText.JsonFormatter.ParsingStrategies
{
    internal sealed class OpenBracket : IStrategy
    {
        public char ForWhichCharacter
        {
            get { return '{'; }
        }

        public void Execute(Context context)
        {
            if (context.IsProcessingString)
            {
                context.AppendCurrentChar();
                return;
            }

            context.AppendCurrentChar();
            context.EnterObjectScope();

            if (!IsBeginningOfNewLineAndIndentionLevel(context))
            {
                return;
            }

            context.BuildContextIndents();
        }

        private static bool IsBeginningOfNewLineAndIndentionLevel(Context context)
        {
            return context.IsProcessingVariableAssignment || (!context.IsStart && !context.IsInArrayScope);
        }
    }
}
