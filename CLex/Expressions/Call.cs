using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Expressions
{
    internal class Call : Expr
    {
        public Expr Callee { get; }
        public Token Paren { get; }
        public List<Expr> Arguments { get; }

        public Call(Expr callee, Token paren, List<Expr> arguments)
        {
            Callee = callee;
            Paren = paren;
            Arguments = arguments;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitCallExpr(this);
        }
    }
}
