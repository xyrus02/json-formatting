namespace DL.PrettyText.JsonFormatterInternals.ParsingStrategies
{
    internal interface IStrategy
    {
        char ForWhichCharacter { get; }

        void Execute(Context context);
    }
}
