using CLex.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLex
{
    internal class AstPrinter : IVisitor<string>
    {
        public string Print(Expr expr)
        {
            return expr.Accept(this);
        }

        public string VisitBinaryExpr(Binary expr)
        {
            return Parenthesize(name: expr.Operator.Lexeme, exprs: new Expr[2] { expr.Left, expr.Right });
        }

        public string VisitGroupingExpr(Grouping expr)
        {
            return Parenthesize(name: "group", exprs: new Expr[1] { expr.Expression });
        }

        public string VisitLiteralExpr(Literal expr)
        {
            if (expr.Value == null)
            {
                return "nil";
            }

            return expr.Value.ToString();
        }

        public string VisitTernaryExpr(Ternary expr)
        {
            throw new NotImplementedException();
        }

        public string VisitUnaryExpr(Unary expr)
        {
            return Parenthesize(name: expr.Operator.Lexeme, exprs: new Expr[1] { expr.Right });
        }

        private string Parenthesize(string name, Expr[] exprs)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(").Append(name);

            foreach (var expr in exprs)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }

            builder.Append(")");

            return builder.ToString();
        }
    }
}
