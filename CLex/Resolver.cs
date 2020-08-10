using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLex.Expressions;
using CLex.Statements;

namespace CLex
{
    enum FunctionType {
        NONE,
        FUNCTION
    }

    internal class Resolver : Expressions.IVisitor<object>, Statements.IVisitor<object>
    {
        Interpreter Interpreter { get; }
        Stack<Dictionary<string, bool>> scopes = new Stack<Dictionary<string, bool>>();
        FunctionType currentFunction = FunctionType.NONE;

        public Resolver(Interpreter interpreter)
        {
            this.Interpreter = interpreter;
        }

        public void Resolve(List<Stmt> statements) {
            foreach (var statement in statements)
            {
                Resolve(statement);   
            }
        }

        void Resolve(Stmt stmt) {
            stmt.Accept(this);
        }

        void Resolve(Expr expr) {
            expr.Accept(this);
        }


        void BeginScope() {
            scopes.Push(new Dictionary<string, bool>());
        }

        void EndScope() {
            scopes.Pop();
        }

        void Declare(Token name) 
        {
            if (scopes.Count == 0) 
            {
                return;
            }

            Dictionary<string, bool> scope = scopes.Peek();

            if (scope.ContainsKey(name.Lexeme)) {
                Lox.Error(name, "Variable with this name already declared in this scope.");
            }

            scope[name.Lexeme] = false;
        }

        void Define(Token name) 
        {
            if (scopes.Count == 0) 
            {
                return;
            }

            scopes.Peek()[name.Lexeme] = true;
        }

        void ResolveLocal(Expr expr, Token name) 
        {
            for (int i = scopes.Count - 1; i >= 0; i--)
            {
                if(scopes.ElementAt(i).ContainsKey(name.Lexeme))
                {
                    Interpreter.Resolve(expr, scopes.Count - 1 - i);
                    return;
                }
            }

            // Not found. Assume it is global.
        }

        object Expressions.IVisitor<object>.VisitAssignExpr(Expressions.Assign expr)
        {
            Resolve(expr.Value);
            ResolveLocal(expr, expr.Name);
            return null;
        }

        object Expressions.IVisitor<object>.VisitBinaryExpr(Expressions.Binary expr)
        {
            Resolve(expr.Left);
            Resolve(expr.Right);
            return null;
        }

        object Statements.IVisitor<object>.VisitBlockStmt(Block stmt)
        {
            BeginScope();
            Resolve(stmt.Statements);
            EndScope();
            return null;
        }
        
        void ResolveFunction(Statements.Function function, FunctionType type) 
        {
            FunctionType enclosingFunction = currentFunction;
            currentFunction = type;
            
            BeginScope();
            foreach (var parameter in function.Params)
            {
                Declare(parameter);
                Define(parameter);   
            }
            Resolve(function.Body);
            EndScope();

            currentFunction = enclosingFunction;
        }

        object Expressions.IVisitor<object>.VisitCallExpr(Expressions.Call expr)
        {
            Resolve(expr.Callee);

            foreach (var argument in expr.Arguments)
            {
                Resolve(argument);
            }

            return null;
        }

        object Statements.IVisitor<object>.VisitExpressionStmt(Expression stmt)
        {
            Resolve(stmt.Expr);
            return null;
        }

        object Statements.IVisitor<object>.VisitFunctionStmt(Function stmt)
        {
            Declare(stmt.Name);
            Define(stmt.Name);

            ResolveFunction(stmt, FunctionType.FUNCTION);
            return null;
        }

        object Expressions.IVisitor<object>.VisitGetExpr(Expressions.Get expr)
        {
            throw new NotImplementedException();
        }

        object Expressions.IVisitor<object>.VisitGroupingExpr(Expressions.Grouping expr)
        {
            Resolve(expr.Expression);
            return null;
        }

        object Statements.IVisitor<object>.VisitIfStmt(If stmt)
        {
            Resolve(stmt.Condition);
            Resolve(stmt.ThenBranch);

            if (stmt.ElseBranch != null) 
            {
                Resolve(stmt.ElseBranch);
            }

            return null;
        }

        object Expressions.IVisitor<object>.VisitLiteralExpr(Expressions.Literal expr)
        {
            return null;
        }

        object Expressions.IVisitor<object>.VisitLogicalExpr(Expressions.Logical expr)
        {
            Resolve(expr.Left);
            Resolve(expr.Right);
            return null;
        }

        object Statements.IVisitor<object>.VisitPrintStmt(Print stmt)
        {
            Resolve(stmt.Expr);
            return null;
        }

        object Statements.IVisitor<object>.VisitReturnStmt(Statements.Return stmt)
        {
            if (currentFunction == FunctionType.NONE) 
            {
                Lox.Error(stmt.Keyword, "Cannot return from top-level code.");
            }

            if (stmt.Value != null) {
                Resolve(stmt.Value);
            }

            return null;
        }

        object Expressions.IVisitor<object>.VisitSetExpr(Expressions.Set expr)
        {
            throw new NotImplementedException();
        }

        object Expressions.IVisitor<object>.VisitTernaryExpr(Expressions.Ternary expr)
        {
            throw new NotImplementedException();
        }

        object Expressions.IVisitor<object>.VisitUnaryExpr(Expressions.Unary expr)
        {
            Resolve(expr.Right);
            return null;
        }

        object Expressions.IVisitor<object>.VisitVariableExpr(Expressions.Variable expr)
        {
            if (scopes.Count > 0 && 
                scopes.Peek().ContainsKey(expr.Name.Lexeme) && 
                scopes.Peek()[expr.Name.Lexeme] == false)
            {
                Lox.Error(expr.Name, "Cannot read local variable in its own initializer.");
            }

            ResolveLocal(expr, expr.Name);
            return null;
        }

        object Statements.IVisitor<object>.VisitVarStmt(Var stmt)
        {
            Declare(stmt.Name);
            if (stmt.Initializer != null) 
            {
                Resolve(stmt.Initializer);
            }
            Define(stmt.Name);
            return null;
        }

        object Statements.IVisitor<object>.VisitWhileStmt(While stmt)
        {
            Resolve(stmt.Condition);
            Resolve(stmt.Body);
            return null;
        }
    }
}
