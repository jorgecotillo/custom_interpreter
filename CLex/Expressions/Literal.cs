using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Expressions
{
    internal class Literal : Expr
    {
        public object Value { get; }

        public Literal(object value)
        {
            Value = value;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }
    }
}
