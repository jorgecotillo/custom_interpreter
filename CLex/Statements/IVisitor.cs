﻿using System;
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
    }
}
