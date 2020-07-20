using System;
using System.Collections.Generic;
using System.Text;

namespace CLex.Statements
{
    internal abstract class Stmt
    {
        public abstract R Accept<R>(IVisitor<R> visitor);
    }
}
