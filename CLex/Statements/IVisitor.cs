using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Statements
{
    internal interface IVisitor<R>
    {
        R VisitPrintStmt(Print stmt);
        R VisitExpressionStmt(Expression stmt);
        R VisitVarStmt(Var stmt);
        R VisitBlockStmt(Block stmt);
        R VisitIfStmt(If stmt);
        R VisitWhileStmt(While stmt);
        R VisitFunctionStmt(Function function);
        R VisitReturnStmt(Return @return);
    }
}
