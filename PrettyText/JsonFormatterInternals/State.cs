namespace DL.PrettyText.JsonFormatterInternals
{
    using System.Collections.Generic;

    internal class State
    {
        private readonly Stack<Scope> scopeStack;

        internal State()
        {
            this.Indentation = string.Empty;
            this.scopeStack = new Stack<Scope>();
        }

        internal enum Scope
        {
            Object,
            Array
        }

        internal string Indentation { get; set; }

        internal char CurrentCharacter { get; set; }

        internal char PreviousChar { get; set; }

        internal bool IsTopTypeArray
        {
            get
            {
                return this.scopeStack.Count > 0
                    && this.scopeStack.Peek() == Scope.Array;
            }
        }

        internal int ScopeDepth
        {
            get
            {
                return this.scopeStack.Count;
            }
        }

        internal void PushObjectContextOntoStack()
        {
            this.scopeStack.Push(Scope.Object);
        }

        internal Scope PopJsonType()
        {
            return this.scopeStack.Pop();
        }

        internal void PushJsonArrayType()
        {
            this.scopeStack.Push(Scope.Array);
        }
    }
}
