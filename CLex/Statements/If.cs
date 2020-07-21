using CLex.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Statements
{
    internal class If : Stmt
    {
        public Expr Condition { get; }
        public Stmt ThenBranch { get; }
        public Stmt ElseBranch { get; }

        public If(Expr condition, Stmt thenBranch, Stmt elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitIfStmt(this);
        }
    }
}
