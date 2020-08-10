using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CLex
{
    internal class Environment
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();
        private Environment Enclosing { get; }

        public Environment()
        {
            Enclosing = null;
        }

        public Environment(Environment enclosing)
        {
            Enclosing = enclosing;
        }

        public object Get(Token name)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                return values[name.Lexeme];
            }

            if (Enclosing != null)
            {
                return Enclosing.Get(name);
            }

            throw new RuntimeError(name,
                "Undefined variable '" + name.Lexeme + "'.");
        }

        public void Define(string name, object value)
        {
            values.Add(name, value);
        }

        Environment Ancestor(int distance) 
        {
            Environment environment = this;

            for (int i = 0; i < distance; i++) 
            {
                environment = environment.Enclosing; 
            }

            return environment;
        }

        public void Assign(Token name, object value)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                values[name.Lexeme] = value;
                return;
            }

            if (Enclosing != null)
            {
                Enclosing.Assign(name, value);
                return;
            }

            throw new RuntimeError(name,
                "Undefined variable '" + name.Lexeme + "'.");
        }

        public object GetAt(int distance, string name)
        {
            return Ancestor(distance).values[name];
        }

        public void AssignAt(int distance, Token name, object value) 
        {
            Ancestor(distance).values[name.Lexeme] = value;
        }
    }
}
