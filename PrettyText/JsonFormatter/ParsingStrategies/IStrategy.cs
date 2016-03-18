namespace DL.PrettyText.JsonFormatter.ParsingStrategies
{
    internal interface IStrategy
    {
        char ForWhichCharacter { get; }

        void Execute(Context context);
    }
}
