using CLex.Expressions;
using CLex.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLex
{
    internal class Interpreter : Expressions.IVisitor<object>, Statements.IVisitor<object>
    {
        public void Interpret(Expr expression)
        {
            try
            {
                object value = Evaluate(expression);
                Console.WriteLine(Stringify(value));
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
            //Evaluate(stmt.Expression);
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
            //object value = Evaluate(stmt.Expression);
            //Console.WriteLine(value);
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
    }
}
