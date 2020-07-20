using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Expressions
{
    internal class Assign : Expr
    {
        public Token Name { get; }
        public Expr Value { get; }

        public Assign(Token name, Expr value)
        {
            Name = name;
            Value = value;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitAssignExpr(this);
        }
    }
}
