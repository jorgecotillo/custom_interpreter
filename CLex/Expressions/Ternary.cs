using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Expressions
{
    internal class Ternary : Expr
    {
        public Binary Left { get; }
        public Token Operator { get; }
        public Expr TrueExpr { get; }
        public Token TrueFalseOperator { get; }
        public Expr FalseExpr { get; }

        public Ternary(Binary left, Token op, Expr trueExpr, Token trueFalseOp, Expr falseExpr)
        {
            Left = left;
            Operator = op;
            TrueExpr = trueExpr;
            TrueFalseOperator = trueFalseOp;
            FalseExpr = falseExpr;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitTernaryExpr(this);
        }
    }
}
