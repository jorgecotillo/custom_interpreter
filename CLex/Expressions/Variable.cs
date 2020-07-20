using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Expressions
{
    internal class Variable : Expr
    {
        public Token Name { get; }

        public Variable(Token name)
        {
            Name = name;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitVariableExpr(this);
        }
    }
}
