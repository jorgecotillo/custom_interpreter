using CLex.Expressions;

namespace CLex.Statements
{
    internal class Print : Stmt
    {
        public Expr Expr { get; }

        public Print(Expr expression)
        {
            this.Expr = expression;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitPrintStmt(this);
        }
    }
}
