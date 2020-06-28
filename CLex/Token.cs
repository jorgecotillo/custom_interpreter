using System;
using System.Collections.Generic;
using System.Text;

namespace CLex
{
    internal class Token
    {
        public readonly TokenType Type;
        public readonly string Lexeme;
        public readonly object Literal;
        public readonly int Line;

        public Token(TokenType type, string lexeme, object literal, int line)
        {
            this.Type = type;
            this.Lexeme = lexeme;
            this.Literal = literal;
            this.Line = line;
        }

        public override string ToString()
        {
            return $"{this.Type} {this.Lexeme} {this.Literal}";
        }
    }
}
