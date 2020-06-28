using CLex.Expressions;
using System;
using System.IO;
using System.Text;

namespace CLex
{
    class Program
    {
        static void Main(string[] args)
        {
            //Expr expression = new Binary(
            //    new Unary(
            //        new Literal(123),
            //        new Token(TokenType.MINUS, "-", null, 1)
            //    ),
            //    new Token(TokenType.STAR, "*", null, 1),
            //    new Grouping(
            //        new Literal(45.67)));

            //Console.WriteLine(new AstPrinter().Print(expression));

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

        internal static void RunFile(string filePath)
        {
            try
            {
                var fileContent = File.ReadAllText(path: filePath);
                Run(fileContent);

            }
            catch (Exception ex)
            {
                Error(line: 10, message: ex.Message);
                Environment.Exit(65);
            }

        }

        internal static void RunPrompt()
        {
            while (true)
            {
                Console.Write("> ");
                try
                {
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
            Expr expression = parser.Parse();

            // Stop if there was a syntax error.
            if (HadError) return;

            Console.WriteLine(new AstPrinter().Print(expression));

            //foreach (var token in tokens)
            //{
            //    Console.WriteLine(token);
            //}
            /*var reader = new StringReader(content);

            while(true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }

                Console.WriteLine(line);
            }*/
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
            var errorMessage = $"jlox: Unexpected {message}, in {where} at line {line}";
            var errorStream = Console.OpenStandardError();
            var bytesErrorMessage = Encoding.ASCII.GetBytes(errorMessage);
            errorStream.Write(buffer: bytesErrorMessage, offset: 0, count: bytesErrorMessage.Length);
            HadError = true;
            Console.WriteLine(Environment.NewLine);
        }
    }
}
