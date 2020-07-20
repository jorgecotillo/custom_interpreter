using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Statements
{
    internal class Block : Stmt
    {
        public List<Stmt> Statements { get; }

        public Block(List<Stmt> statements)
        {
            Statements = statements;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitBlockStmt(this);
        }
    }
}
