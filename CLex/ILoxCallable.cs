using System.Collections.Generic;

namespace CLex
{
    internal interface ILoxCallable
    {
        int Arity();
        object Call(Interpreter interpreter, List<object> arguments);
    }
}          
