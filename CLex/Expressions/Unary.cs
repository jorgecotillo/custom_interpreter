using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Expressions
{
    internal class Unary : Expr
    {
        public Expr Right { get; }
        public Token Operator { get; }

        public Unary(Expr right, Token op)
        {
            Right = right;
            Operator = op;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }
}
