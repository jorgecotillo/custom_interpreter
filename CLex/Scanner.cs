using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading;

namespace CLex
{
    internal class Scanner
    {
        readonly string Source;
        readonly List<Token> Tokens = new List<Token>();
        Dictionary<string, TokenType> Keywords;

        int Start = 0;
        int Current = 0;
        int Line = 1;

        public Scanner(string source)
        {
            this.Source = source;
            FillKeywords();
        }

        void FillKeywords()
        {
            Keywords = new Dictionary<string, TokenType>
            {
                { "and", TokenType.AND },
                { "class", TokenType.CLASS },
                { "else", TokenType.ELSE },
                { "false", TokenType.FALSE },
                { "for", TokenType.FOR },
                { "fun", TokenType.FUN },
                { "if", TokenType.IF },
                { "nil", TokenType.NIL },
                { "or", TokenType.OR },
                { "print", TokenType.PRINT },
                { "return", TokenType.RETURN },
                { "super", TokenType.SUPER },
                { "this", TokenType.THIS },
                { "true", TokenType.TRUE },
                { "var", TokenType.VAR },
                { "while", TokenType.WHILE }
            };
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                Start = Current;
                ScanToken();
            }

            Tokens.Add(new Token(type: TokenType.EOF, lexeme: "", literal: null, line: Line));
            return Tokens;
        }

        bool IsAtEnd()
        {
            return Current >= Source.Length;
        }

        void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '*': AddToken(TokenType.STAR); break;
                case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '"': ProcessString(); break;
                case '/':
                    if (Match('/'))
                    {
                        // A comment goes until the end of the line.
                        while (Peek() != '\n' && !IsAtEnd())
                        {
                            Advance();
                        }
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.
                    break;
                case '\n':
                    Line++;
                    break;
                default:
                    if (IsDigit(c))
                    {
                        ProcessNumber();
                    } 
                    else if (IsAlpha(c))
                    {
                        ProcessIdentifier();
                    }
                    else
                    {
                        Lox.Error(Line, "Unexpected character.");
                    }
                    break;
            }
        }

        char Peek()
        {
            if (IsAtEnd())
            {
                return '\0';
            }

            return Source[Current];
        }

        bool Match(char expected)
        {
            if (IsAtEnd())
            {
                return false;
            }

            if (Source[Current] != expected)
            {
                return false;
            }

            Current++;
            return true;
        }

        char Advance()
        {
            Current++;
            return Source[Current - 1];
        }

        void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        void AddToken(TokenType type, object literal)
        {
            var text = Source[Start..Current];
            Tokens.Add(new Token(type: type, lexeme: text, literal: literal, line: Line));
        }

        void ProcessString()
        {
            while (!IsEndOfString(Peek()))
            {
                if (Peek() == '\n')
                {
                    Line++;
                }

                Advance();
            }

            // Unterminated string
            if (IsAtEnd())
            {
                Lox.Error(Line, "Unterminated string.");
                return;
            }

            // The closing "
            Advance();

            // Trim the surrounding quotes
            var value = Source[(Start + 1)..(Current - 1)];
            AddToken(TokenType.STRING, value);
        }

        bool IsEndOfString(char c)
        {
            if (IsAtEnd())
            {
                return true;
            }

            if (c != '\\' && c != '"')
            {
                return false;
            }

            if(c == '\\' || (c == '"' && PeekPrevious() == '\\'))
            {
                return false;
            }

            return true;
        }

        bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        void ProcessNumber()
        {
            while (IsDigit(Peek()))
            {
                Advance();
            }

            // Look for a fractional part
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                // Consume the .
                Advance();

                while (IsDigit(Peek()))
                {
                    Advance();
                }
            }

            AddToken(TokenType.NUMBER, double.Parse(Source[Start..Current]));
        }

        char PeekNext()
        {
            if (Current + 1 >= Source.Length)
            {
                return '\0';
            }

            return Source[Current + 1];
        }

        char PeekPrevious()
        {
            if (Current - 1 < 0)
            {
                return '\0';
            }

            return Source[Current - 1];
        }

        void ProcessIdentifier()
        {
            while (IsAlphaNumeric(Peek()))
            {
                Advance();
            }

            var text = Source[Start..Current];

            if (!Keywords.ContainsKey(text))
            {
                AddToken(TokenType.IDENTIFIER);
            }
            else
            {
                AddToken(Keywords[text]);
            }
            
        }

        bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
        }

        bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }
    }
}
