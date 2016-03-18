namespace DL.PrettyText.JsonFormatterInternals.ParsingStrategies
{
    using System;

    internal sealed class DefaultCharacter : IStrategy
    {
        public char ForWhichCharacter
        {
            get
            {
                throw new InvalidOperationException("This strategy is not intended for any particular character.");
            }
        }

        public void Execute(Context context)
        {
            context.AppendCurrentChar();
        }
    }
}
