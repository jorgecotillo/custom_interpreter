using CLex.Expressions;
using CLex.Statements;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CLex
{
    class Clock : ILoxCallable
    {
        public int Arity()
        {
            return 0;
        }

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            return (double)System.DateTime.Now.Millisecond / 1000.0;
        }

        public override string ToString()
        {
            return "<system function>";
        }
    }
    internal class Interpreter : Expressions.IVisitor<object>, Statements.IVisitor<object>
    {
        public static Environment Globals = new Environment();
        private Environment environment = Globals;
        private Dictionary<Expr, int> locals = new Dictionary<Expr, int>();

        public Interpreter() {
            Globals.Define("clock", new Clock());
        }

        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (var statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (RuntimeError error)
            {
                Lox.RuntimeError(error);
            }
        }

        string Stringify(object obj)
        {
            if (obj == null) return "nil";

            // Hack. Work around by adding ".0" to integer-valued doubles.
            if (obj is double) {
                string text = obj.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
                return text;
            }

            return obj.ToString();
        }

        object Expressions.IVisitor<object>.VisitBinaryExpr(Binary expr)
        {
            object left = Evaluate(expr.Left);
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.MINUS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left - (double)right;
                case TokenType.PLUS:
                    if (left is double @double && right is double double1) {
                        return @double + double1;
                    }

                    if (left is string && right is string) {
                        return string.Concat(left, right);
                    }

                    throw new RuntimeError(expr.Operator, "Operands must be two numbers or two strings.");
                case TokenType.SLASH:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left * (double)right;
                case TokenType.BANG_EQUAL:
                    return !IsEqual(left, right);
                case TokenType.EQUAL_EQUAL:
                    return IsEqual(left, right);
                case TokenType.GREATER:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left <= (double)right;
                default:
                    break;
            }

            // Unreachable.
            return null;
        }

        public object VisitCallExpr(Call expr) {
            object callee = Evaluate(expr.Callee);

            List<object> arguments = new List<object>();
            
            foreach(Expr argument in expr.Arguments)
            {
                arguments.Add(Evaluate(argument));
            }
            
            if (!(callee is ILoxCallable)) {
                throw new RuntimeError(expr.Paren, "Can only call functions and classes.");
            }
            var function = callee as ILoxCallable;

            if (arguments.Count != function.Arity()) {
                throw new RuntimeError(expr.Paren, $"Expected {function.Arity()} arguments but got {arguments.Count}.");
            }

            return function.Call(this, arguments);
        }

        object Statements.IVisitor<object>.VisitExpressionStmt(Expression stmt)
        {
            Evaluate(stmt.Expr);
            return null;
        }

        object Expressions.IVisitor<object>.VisitGroupingExpr(Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        object Expressions.IVisitor<object>.VisitLiteralExpr(Literal expr)
        {
            return expr.Value;
        }

        object Statements.IVisitor<object>.VisitPrintStmt(Print stmt)
        {
            object value = Evaluate(stmt.Expr);
            Console.WriteLine(Stringify(value));
            return null;
        }

        object Expressions.IVisitor<object>.VisitTernaryExpr(Ternary expr)
        {
            throw new NotImplementedException();
        }

        object Expressions.IVisitor<object>.VisitUnaryExpr(Unary expr)
        {
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.MINUS:
                    CheckNumberOperand(expr.Operator, right);
                    return -(double)right;
                case TokenType.BANG:
                    return !IsTruthy(right);
                default:
                    break;
            }

            // Unreachable.
            return null;
        }

        private object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is bool boolean) return boolean;
            return true;
        }

        private bool IsEqual(object a, object b)
        {
            // nil is only equal to nil.
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }

        private void CheckNumberOperand(Token op, object operand)
        {
            if (operand is double) return;
            throw new RuntimeError(op, "Operand must be a number.");
        }

        private void CheckNumberOperands(Token op, object left, object right)
        {
            if (left is double && right is double) return;

            throw new RuntimeError(op, "Operands must be numbers.");
        }

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }

        public void Resolve(Expr expr, int depth) {
            locals[expr] = depth;
        }

        object Statements.IVisitor<object>.VisitVarStmt(Statements.Var stmt)
        {
            object value = null;
            if (stmt.Initializer != null)
            {
                value = Evaluate(stmt.Initializer);
            }

            environment.Define(stmt.Name.Lexeme, value);
            return null;
        }

        object Expressions.IVisitor<object>.VisitVariableExpr(Variable expr)
        {
            return LookUpVariable(expr.Name, expr);
        }

        private object LookUpVariable(Token name, Expr expr) 
        {
            if (locals.ContainsKey(expr)) 
            {
                int distance = locals[expr];
                return environment.GetAt(distance, name.Lexeme);
            } 
            else 
            {
                return Globals.Get(name);
            }
        }

        object Expressions.IVisitor<object>.VisitAssignExpr(Assign expr)
        {
            object value = Evaluate(expr.Value);
            
            if (locals.ContainsKey(expr)) 
            {
                int distance = locals[expr];
                environment.AssignAt(distance, expr.Name, value);
            }
            else 
            {
                Globals.Assign(expr.Name, value);
            }

            return value;
        }

        object Expressions.IVisitor<object>.VisitGetExpr(Get get)
        {
            throw new NotImplementedException();
        }

        object Expressions.IVisitor<object>.VisitCallExpr(Call expr)
        {
            object callee = Evaluate(expr.Callee);

            List<object> arguments = new List<object>();
            foreach (Expr argument in expr.Arguments)
            {
                arguments.Add(Evaluate(argument));
            }

            var function = callee as ILoxCallable;
            return function.Call(this, arguments);
        }

        object Statements.IVisitor<object>.VisitBlockStmt(Block stmt)
        {
            ExecuteBlock(stmt.Statements, new Environment(environment));
            return null;
        }

        public void ExecuteBlock(List<Stmt> statements, Environment environment)
        {
            Environment previous = this.environment;
            try
            {
                this.environment = environment;
                foreach (var statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                this.environment = previous;
            }
        }

        object Statements.IVisitor<object>.VisitIfStmt(If stmt)
        {
            if (IsTruthy(Evaluate(stmt.Condition)))
            {
                Execute(stmt.ThenBranch);
            }
            else if (stmt.ElseBranch != null)
            {
                Execute(stmt.ElseBranch);
            }
            return null;
        }

        object Expressions.IVisitor<object>.VisitSetExpr(Set expr)
        {
            throw new NotImplementedException();
        }

        object Expressions.IVisitor<object>.VisitLogicalExpr(Logical expr)
        {
            object left = Evaluate(expr.Left);

            if (expr.Operator.Type == TokenType.OR) {
                if (IsTruthy(left)) return left;
            } else
            {
                if (!IsTruthy(left)) return left;
            }

            return Evaluate(expr.Right);
        }

        object Statements.IVisitor<object>.VisitWhileStmt(While stmt)
        {
            while (IsTruthy(Evaluate(stmt.Condition)))
            {
                Execute(stmt.Body);
            }
            return null;
        }

        public object VisitFunctionStmt(Function stmt)
        {
            LoxFunction function = new LoxFunction(stmt, environment);
            environment.Define(stmt.Name.Lexeme, function);
            return null;
        }

        public object VisitReturnStmt(Statements.Return stmt)
        {
            object value = null;
            if (stmt.Value != null) value = Evaluate(stmt.Value);

            throw new CLex.Return(value);
        }
    }
}
