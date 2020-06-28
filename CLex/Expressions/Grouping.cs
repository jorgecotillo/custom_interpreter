using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CLex.Expressions
{
    internal class Grouping : Expr
    {
        public Expr Expression { get; }

        public Grouping(Expr expression)
        {
            Expression = expression;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitGroupingExpr(this);
        }
    }
}
