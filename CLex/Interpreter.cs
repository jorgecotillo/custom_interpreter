using CLex.Expressions;
using CLex.Statements;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CLex
{
    internal class Interpreter : Expressions.IVisitor<object>, Statements.IVisitor<object>
    {
        private Environment environment = new Environment();

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
                case TokenType.LEFT_PAREN:
                    break;
                case TokenType.RIGHT_PAREN:
                    break;
                case TokenType.LEFT_BRACE:
                    break;
                case TokenType.RIGHT_BRACE:
                    break;
                case TokenType.COMMA:
                    break;
                case TokenType.DOT:
                    break;
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

                case TokenType.SEMICOLON:
                    break;
                case TokenType.SLASH:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left * (double)right;
                case TokenType.BANG:
                    break;
                case TokenType.BANG_EQUAL:
                    return !IsEqual(left, right);
                case TokenType.EQUAL:
                    break;
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
                case TokenType.IDENTIFIER:
                    break;
                case TokenType.STRING:
                    break;
                case TokenType.NUMBER:
                    break;
                case TokenType.AND:
                    break;
                case TokenType.CLASS:
                    break;
                case TokenType.ELSE:
                    break;
                case TokenType.FALSE:
                    break;
                case TokenType.FUN:
                    break;
                case TokenType.FOR:
                    break;
                case TokenType.IF:
                    break;
                case TokenType.NIL:
                    break;
                case TokenType.OR:
                    break;
                case TokenType.PRINT:
                    break;
                case TokenType.RETURN:
                    break;
                case TokenType.SUPER:
                    break;
                case TokenType.THIS:
                    break;
                case TokenType.TRUE:
                    break;
                case TokenType.VAR:
                    break;
                case TokenType.WHILE:
                    break;
                case TokenType.EOF:
                    break;
                default:
                    break;
            }

            // Unreachable.
            return null;
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
                case TokenType.LEFT_PAREN:
                    break;
                case TokenType.RIGHT_PAREN:
                    break;
                case TokenType.LEFT_BRACE:
                    break;
                case TokenType.RIGHT_BRACE:
                    break;
                case TokenType.COMMA:
                    break;
                case TokenType.DOT:
                    break;
                case TokenType.MINUS:
                    CheckNumberOperand(expr.Operator, right);
                    return -(double)right;
                case TokenType.PLUS:
                    break;
                case TokenType.SEMICOLON:
                    break;
                case TokenType.SLASH:
                    break;
                case TokenType.STAR:
                    break;
                case TokenType.BANG:
                    return !IsTruthy(right);
                case TokenType.BANG_EQUAL:
                    break;
                case TokenType.EQUAL:
                    break;
                case TokenType.EQUAL_EQUAL:
                    break;
                case TokenType.GREATER:
                    break;
                case TokenType.GREATER_EQUAL:
                    break;
                case TokenType.LESS:
                    break;
                case TokenType.LESS_EQUAL:
                    break;
                case TokenType.IDENTIFIER:
                    break;
                case TokenType.STRING:
                    break;
                case TokenType.NUMBER:
                    break;
                case TokenType.AND:
                    break;
                case TokenType.CLASS:
                    break;
                case TokenType.ELSE:
                    break;
                case TokenType.FALSE:
                    break;
                case TokenType.FUN:
                    break;
                case TokenType.FOR:
                    break;
                case TokenType.IF:
                    break;
                case TokenType.NIL:
                    break;
                case TokenType.OR:
                    break;
                case TokenType.PRINT:
                    break;
                case TokenType.RETURN:
                    break;
                case TokenType.SUPER:
                    break;
                case TokenType.THIS:
                    break;
                case TokenType.TRUE:
                    break;
                case TokenType.VAR:
                    break;
                case TokenType.WHILE:
                    break;
                case TokenType.EOF:
                    break;
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
            return environment.Get(expr.Name);
        }

        object Expressions.IVisitor<object>.VisitAssignExpr(Assign expr)
        {
            object value = Evaluate(expr.Value);
            environment.Assign(expr.Name, value);
            return value;
        }

        object Expressions.IVisitor<object>.VisitGetExpr(Get get)
        {
            throw new NotImplementedException();
        }

        object Expressions.IVisitor<object>.VisitCallExpr(Call call)
        {
            throw new NotImplementedException();
        }

        object Statements.IVisitor<object>.VisitBlockStmt(Block stmt)
        {
            ExecuteBlock(stmt.Statements, new Environment(environment));
            return null;
        }

        void ExecuteBlock(List<Stmt> statements, Environment environment)
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
    }
}
