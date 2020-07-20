using System;
using System.Collections.Generic;
using System.Text;

namespace CLex
{
    internal class RuntimeError : Exception
    {
        public Token Token { get; }

        public RuntimeError(Token token, string message) : base(message: message)
        {
            this.Token = token;
        }
    }
}
