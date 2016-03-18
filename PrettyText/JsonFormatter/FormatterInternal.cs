namespace DL.PrettyText.JsonFormatter
{
    using System.Text;
    using DL.PrettyText.JsonFormatter.ParsingStrategies;

    internal sealed class FormatterInternal
    {
        private readonly Context context;

        internal FormatterInternal(ushort indent)
        {
            this.context = new Context(indent);
            this.context.ClearStrategies();
            this.context.AddCharacterStrategy(new OpenBracket());
            this.context.AddCharacterStrategy(new CloseBracket());
            this.context.AddCharacterStrategy(new OpenSquareBracket());
            this.context.AddCharacterStrategy(new CloseSquareBracket());
            this.context.AddCharacterStrategy(new SingleQuote());
            this.context.AddCharacterStrategy(new DoubleQuote());
            this.context.AddCharacterStrategy(new Comma());
            this.context.AddCharacterStrategy(new ColonCharacter());
            this.context.AddCharacterStrategy(new SkipWhileNotInString('\n'));
            this.context.AddCharacterStrategy(new SkipWhileNotInString('\r'));
            this.context.AddCharacterStrategy(new SkipWhileNotInString('\t'));
            this.context.AddCharacterStrategy(new SkipWhileNotInString(' '));
        }

        internal string Format(string json)
        {
            if (json == null)
            {
                return string.Empty;
            }

            if (json.Trim() == string.Empty)
            {
                return string.Empty;
            }

            var input = new StringBuilder(json);
            var output = new StringBuilder();

            this.PrettyPrintCharacter(input, output);

            return output.ToString();
        }

        private void PrettyPrintCharacter(StringBuilder input, StringBuilder output)
        {
            for (var i = 0; i < input.Length; i++)
            {
                this.context.PrettyPrintCharacter(input[i], output);
            }
        }
    }
}
