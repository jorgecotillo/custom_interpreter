using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Expressions
{
    internal class Logical : Expr
    {
        public Expr Left { get; }
        public Token Operator { get; }
        public Expr Right { get; }

        public Logical(Expr left, Token op, Expr right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitLogicalExpr(this);
        }
    }
}
