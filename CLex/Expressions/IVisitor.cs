using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Expressions
{
    internal interface IVisitor<R>
    {
        R VisitBinaryExpr(Binary expr);
        R VisitGroupingExpr(Grouping expr);
        R VisitLiteralExpr(Literal expr);
        R VisitUnaryExpr(Unary expr);
        R VisitTernaryExpr(Ternary expr);
    }
}
