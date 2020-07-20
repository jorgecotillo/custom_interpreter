using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Expressions
{
    internal class Get : Expr
    {
        public Expr Object { get; }
        public Token Name { get; }

        public Get(Expr obj, Token name)
        {
            Object = obj;
            Name = name;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitGetExpr(this);
        }
    }
}
