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
        R VisitVariableExpr(Variable expr);
        R VisitAssignExpr(Assign expr);
        R VisitGetExpr(Get expr);
        R VisitCallExpr(Call expr);
        R VisitSetExpr(Set expr);
        R VisitLogicalExpr(Logical expr);
    }
}
