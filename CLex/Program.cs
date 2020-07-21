using CLex.Expressions;
using CLex.Statements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLex
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: jlox [script]");
            }
            else if(args.Length == 1)
            {
                Lox.RunFile(args[0]);
            }
            else
            {
                Lox.RunPrompt();
            }
        }
    }

    internal class Lox
    {
        private static bool HadError;
        private static bool HadRuntimeError;
        private static readonly Interpreter Interpreter = new Interpreter();

        internal static void RunFile(string filePath)
        {
            try
            {
                // Reset flags
                HadError = false;
                HadRuntimeError = false;

                var fileContent = File.ReadAllText(path: filePath);
                Run(fileContent);
            }
            catch (Exception ex)
            {
                Error(line: 10, message: ex.Message);
                System.Environment.Exit(65);
            }

        }

        internal static void RunPrompt()
        {
            while (true)
            {
                Console.Write("> ");
                try
                {
                    // Reset flags
                    HadError = false;
                    HadRuntimeError = false;

                    Run(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Error(line: 10, message: ex.Message);
                    HadError = false;
                }
            }
        }

        internal static void Run(string content)
        {

            var scanner = new Scanner(content);
            var tokens = scanner.ScanTokens();

            Parser parser = new Parser(tokens);
            List<Stmt> statements = parser.Parse();

            // Stop if there was a syntax error.
            if (HadError) return;
            if (HadRuntimeError) return;
            
            Interpreter.Interpret(statements);
        }

        internal static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        internal static void Error(Token token, String message)
        {
            if (token.Type == TokenType.EOF)
            {
                Report(token.Line, " at end", message);
            }
            else
            {
                Report(token.Line, " at '" + token.Lexeme + "'", message);
            }
        }

        internal static void Report(int line, string where, string message)
        {
            var errorMessage = $"jlox: error: {message}, at line {line}";
            var errorStream = Console.OpenStandardError();
            var bytesErrorMessage = Encoding.ASCII.GetBytes(errorMessage);
            errorStream.Write(buffer: bytesErrorMessage, offset: 0, count: bytesErrorMessage.Length);

            HadError = true;
            Console.WriteLine(System.Environment.NewLine);
        }

        internal static void RuntimeError(RuntimeError error)
        {
            var errorMessage = $"jlox: error: {error.Message}, at line {error.Token.Line}";
            var errorStream = Console.OpenStandardError();
            var bytesErrorMessage = Encoding.ASCII.GetBytes(errorMessage);
            errorStream.Write(buffer: bytesErrorMessage, offset: 0, count: bytesErrorMessage.Length);

            HadRuntimeError = true;
            Console.WriteLine(System.Environment.NewLine);
        }
    }
}
