using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Expressions
{
    internal class Set : Expr
    {
        public Expr Object { get; }
        public Token Name { get; }
        public Expr Value { get; }

        public Set(Expr obj, Token name, Expr value)
        {
            Object = obj;
            Name = name;
            Value = value;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitSetExpr(this);
        }
    }
}
