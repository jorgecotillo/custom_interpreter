using System.Collections.Generic;
using CLex.Expressions;

namespace CLex.Statements
{
    internal class Function : Stmt
    {
        public Token Name { get; }
        public List<Token> Params { get; }
        public List<Statements.Stmt> Body { get; }

        public Function(Token name, List<Token> parameters, List<Stmt> body)
        {
            this.Name = name;
            this.Params = parameters;
            this.Body = body;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitFunctionStmt(this);
        }
    }
}
