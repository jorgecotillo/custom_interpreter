using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Expressions
{
    internal abstract class Expr
    {
        public abstract R Accept<R>(IVisitor<R> visitor);
    }
}
