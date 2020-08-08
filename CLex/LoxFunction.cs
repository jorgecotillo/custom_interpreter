using System.Collections.Generic;
using CLex;
using CLex.Statements;

namespace CLex
{
    internal class LoxFunction : ILoxCallable
    {
        Function Declaration { get; }
        Environment Closure { get; }
        
        public LoxFunction(Function declaration, Environment closure)
        {
            this.Closure = closure;
            this.Declaration = declaration;
        }

        public int Arity()
        {
            return Declaration.Params.Count;
        }

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            Environment environment = new Environment(Closure);
            for (int i = 0; i < Declaration.Params.Count; i++) 
            {
                environment.Define(Declaration.Params[i].Lexeme, arguments[i]);
            }

            try
            {
                interpreter.ExecuteBlock(Declaration.Body, environment);
                return null;
            }
            catch (Return returnValue)
            {
                return returnValue.Value;
            }
        }

        public override string ToString()
        {
            return $"<function {Declaration.Name.Lexeme}>";
        }
    }
}