using CLex.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Statements
{
    internal class Return : Stmt
    {
        public Token Keyword { get; }
        public Expr Value { get; }

        public Return(Token keyword, Expr value)
        {
            Keyword = keyword;
            Value = value;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitReturnStmt(this);
        }
    }
}
