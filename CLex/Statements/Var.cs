using CLex.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Statements
{
    internal class Var : Stmt
    {
        public Token Name { get; }
        public Expr Initializer { get; }

        public Var(Token name, Expr initializer)
        {
            Name = name;
            Initializer = initializer;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitVarStmt(this);
        }
    }
}
