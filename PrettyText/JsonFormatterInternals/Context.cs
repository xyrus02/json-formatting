namespace DL.PrettyText.JsonFormatterInternals
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DL.PrettyText.JsonFormatterInternals.ParsingStrategies;

    internal sealed class Context
    {
        private const char Space = ' ';
        private readonly int spacesPerIndent;
        private readonly State state;
        private readonly IDictionary<char, IStrategy> strategies;
        private StringBuilder outputBuilder;

        internal Context(ushort indent)
        {
            this.spacesPerIndent = indent;
            this.state = new State();
            this.strategies = new Dictionary<char, IStrategy>();
        }

        internal string Indent
        {
            get
            {
                if (this.state.Indentation == string.Empty)
                {
                    this.InitializeIndent();
                }

                return this.state.Indentation;
            }
        }

        internal bool IsInArrayScope
        {
            get
            {
                return this.state.IsTopTypeArray;
            }
        }

        internal bool IsProcessingVariableAssignment { get; set; }

        internal bool IsProcessingDoubleQuoteInitiatedString { get; set; }

        internal bool IsProcessingSingleQuoteInitiatedString { get; set; }

        internal bool IsProcessingString
        {
            get
            {
                return this.IsProcessingDoubleQuoteInitiatedString
                    || this.IsProcessingSingleQuoteInitiatedString;
            }
        }

        internal bool IsStart
        {
            get
            {
                return this.outputBuilder.Length == 0;
            }
        }

        internal bool WasLastCharacterABackSlash
        {
            get
            {
                return this.state.PreviousChar == '\\';
            }
        }

        internal void PrettyPrintCharacter(char curChar, StringBuilder output)
        {
            this.state.CurrentCharacter = curChar;

            IStrategy strategy = this.strategies.ContainsKey(curChar)
                ? this.strategies[curChar]
                : new DefaultCharacter();

            this.outputBuilder = output;

            strategy.Execute(this);

            this.state.PreviousChar = curChar;
        }

        internal void AppendCurrentChar()
        {
            this.outputBuilder.Append(this.state.CurrentCharacter);
        }

        internal void AppendNewLine()
        {
            this.outputBuilder.Append(Environment.NewLine);
        }

        internal void BuildContextIndents()
        {
            this.AppendNewLine();
            this.AppendIndents(this.state.ScopeDepth);
        }

        internal void EnterObjectScope()
        {
            this.state.PushObjectContextOntoStack();
        }

        internal void CloseCurrentScope()
        {
            this.state.PopJsonType();
        }

        internal void EnterArrayScope()
        {
            this.state.PushJsonArrayType();
        }

        internal void AppendSpace()
        {
            this.outputBuilder.Append(Space);
        }

        internal void ClearStrategies()
        {
            this.strategies.Clear();
        }

        internal void AddCharacterStrategy(IStrategy strategy)
        {
            this.strategies[strategy.ForWhichCharacter] = strategy;
        }

        private void InitializeIndent()
        {
            this.state.Indentation += new string(Space, this.spacesPerIndent);
        }

        private void AppendIndents(int indents)
        {
            for (int i = 0; i < indents; i++)
            {
                this.outputBuilder.Append(this.Indent);
            }
        }
    }
}
