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

        public string VisitAssignExpr(Assign expr)
        {
            throw new NotImplementedException();
        }

        public string VisitBinaryExpr(Binary expr)
        {
            return Parenthesize(name: expr.Operator.Lexeme, exprs: new Expr[2] { expr.Left, expr.Right });
        }

        public string VisitCallExpr(Call call)
        {
            throw new NotImplementedException();
        }

        public string VisitGetExpr(Get get)
        {
            throw new NotImplementedException();
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

        string IVisitor<string>.VisitBinaryExpr(Binary expr)
        {
            throw new NotImplementedException();
        }

        string IVisitor<string>.VisitGroupingExpr(Grouping expr)
        {
            throw new NotImplementedException();
        }

        string IVisitor<string>.VisitLiteralExpr(Literal expr)
        {
            throw new NotImplementedException();
        }

        string IVisitor<string>.VisitTernaryExpr(Ternary expr)
        {
            throw new NotImplementedException();
        }

        string IVisitor<string>.VisitUnaryExpr(Unary expr)
        {
            throw new NotImplementedException();
        }

        string IVisitor<string>.VisitVariableExpr(Variable expr)
        {
            throw new NotImplementedException();
        }
    }
}
