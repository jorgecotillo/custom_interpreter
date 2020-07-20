using CLex.Expressions;

namespace CLex.Statements
{
    internal class Expression : Stmt
    {
        public Expr Expr { get; }

        public Expression(Expr expression)
        {
            this.Expr = expression;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitExpressionStmt(this);
        }
    }
}
