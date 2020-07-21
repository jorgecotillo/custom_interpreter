using CLex.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Statements
{
    internal class While : Stmt
    {
        public Expr Condition { get; }
        public Stmt Body { get; }

        public While(Expr condition, Stmt body)
        {
            Condition = condition;
            Body = body;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitWhileStmt(this);
        }
    }
}
