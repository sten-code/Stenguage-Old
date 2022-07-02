using Stenguage.Errors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stenguage
{
    public class Lexer
    {
        public string Text;
        public Position Position;
        public char CurrentChar;

        public string FileName;

        public Lexer(string fileName, string text)
        {
            FileName = fileName;
            Text = text;
            Position = new Position(-1, 0, -1, fileName, text);
            CurrentChar = char.MaxValue;
            Advance();
        }

        public void Advance()
        {
            Position.Advance(CurrentChar);
            CurrentChar = Position.Index < Text.Length ? Text[Position.Index] : char.MaxValue;
        }

        public (List<Token>, Error) MakeTokens()
        {
            List<Token> tokens = new List<Token>();

            while (CurrentChar != char.MaxValue)
            {
                if (" \t".Contains(CurrentChar))
                {
                    Advance();
                } 
                else if (CurrentChar.Equals(';') || CurrentChar.Equals('\n') || (int)CurrentChar == 13)
                {
                    tokens.Add(new Token(Token.TT_NEWLINE, start: Position));
                    Advance();
                }
                else if (char.IsNumber(CurrentChar))
                {
                    tokens.Add(MakeNumber());
                }
                else if (char.IsLetter(CurrentChar) || CurrentChar == '_')
                {
                    tokens.Add(MakeIdentifier());
                }
                else if (CurrentChar == '"')
                {
                    tokens.Add(MakeString());
                }
                else if (CurrentChar == '+')
                {
                    tokens.Add(new Token(Token.TT_PLUS, start: Position));
                    Advance();
                }
                else if (CurrentChar == '-')
                {
                    tokens.Add(MakeMinusOrArrow());
                }
                else if (CurrentChar == '*')
                {
                    tokens.Add(new Token(Token.TT_MUL, start: Position));
                    Advance();
                }
                else if (CurrentChar == '/')
                {
                    tokens.Add(new Token(Token.TT_DIV, start: Position));
                    Advance();
                }
                else if (CurrentChar == '^')
                {
                    tokens.Add(new Token(Token.TT_POW, start: Position));
                    Advance();
                }
                else if (CurrentChar == '(')
                {
                    tokens.Add(new Token(Token.TT_LPAREN, start: Position));
                    Advance();
                }
                else if (CurrentChar == ')')
                {
                    tokens.Add(new Token(Token.TT_RPAREN, start: Position));
                    Advance();
                }
                else if (CurrentChar == '[')
                {
                    tokens.Add(new Token(Token.TT_LBRACK, start: Position));
                    Advance();
                }
                else if (CurrentChar == ']')
                {
                    tokens.Add(new Token(Token.TT_RBRACK, start: Position));
                    Advance();
                }
                else if (CurrentChar == '!')
                {
                    (Token token, Error error) = MakeNotEquals();
                    if (error != null) return (new List<Token>(), error);
                    tokens.Add(token);
                }
                else if (CurrentChar == '=')
                {
                    tokens.Add(MakeEquals());
                }
                else if (CurrentChar == '<')
                {
                    tokens.Add(MakeLessThan());
                }
                else if (CurrentChar == '>')
                {
                    tokens.Add(MakeGreaterThan());
                }
                else if (CurrentChar == ',')
                {
                    tokens.Add(new Token(Token.TT_COMMA, start: Position));
                    Advance();
                }
                else
                {
                    Position start = Position.Copy();
                    char c = CurrentChar;
                    Console.WriteLine((int)CurrentChar);
                    Advance();
                    return (new List<Token>(), new IllegalCharError(start, Position, $"'{c}'"));
                }
            }

            tokens.Add(new Token(Token.TT_EOF, start: Position));
            return (tokens, null);
        }

        public Token MakeString()
        {
            string str = "";
            Position start = Position.Copy();
            bool escape = false;
            Advance();

            Dictionary<char, char> escapeCharacters = new Dictionary<char, char>
            {
                ['n'] = '\n',
                ['t'] = '\t',
            };

            while (CurrentChar != char.MaxValue && (CurrentChar != '"' || escape))
            {
                if (escape)
                {
                    str += escapeCharacters.ContainsKey(CurrentChar) ? escapeCharacters[CurrentChar] : CurrentChar;
                }
                else
                {
                    if (CurrentChar == '\\')
                    {
                        escape = true;
                        Advance();
                        continue;
                    }
                    else
                    {
                        str += CurrentChar;
                    }
                }
                Advance();
                escape = false;
            }
            Advance();
            return new Token(Token.TT_STRING, str, start: start, end: Position);
        }

        public Token MakeMinusOrArrow()
        {
            string tokenType = Token.TT_MINUS;
            Position start = Position.Copy();
            Advance();

            if (CurrentChar == '>')
            {
                Advance();
                tokenType = Token.TT_ARROW;
            }

            return new Token(tokenType, start: start, end: Position);
        }

        public Token MakeNumber()
        {
            string numStr = "";
            int dotCount = 0;
            Position start = Position.Copy();

            while (CurrentChar != char.MaxValue && (char.IsNumber(CurrentChar) || CurrentChar == '.'))
            {
                if (CurrentChar == '.')
                {
                    if (dotCount == 1) break;
                    dotCount++;
                    numStr += '.';
                }
                else
                {
                    numStr += CurrentChar;
                }
                Advance();
            }

            if (dotCount == 0) return new Token(Token.TT_INT, numStr, start: start, end: Position);
            else return new Token(Token.TT_FLOAT, numStr, start: start, end: Position);
        }

        public Token MakeIdentifier()
        {
            string idStr = "";
            Position start = Position.Copy();

            while (CurrentChar != char.MaxValue && (char.IsLetter(CurrentChar) || char.IsNumber(CurrentChar) || CurrentChar == '_' || CurrentChar == '.'))
            {
                idStr += CurrentChar;
                Advance();
            }

            string tokenType = Token.KEYWORDS.Contains(idStr) ? Token.TT_KEYWORD : Token.TT_IDENTIFIER;
            return new Token(tokenType, idStr, start, Position);
        }

        public (Token, Error) MakeNotEquals()
        {
            Position start = Position.Copy();
            Advance();

            if (CurrentChar == '=')
            {
                Advance();
                return (new Token(Token.TT_NE, start: start, end: Position), null);
            }

            Advance();
            return (null, new ExpectedCharError(start, Position, "'=' (after '!')"));
        }

        public Token MakeEquals()
        {
            Position start = Position.Copy();
            Advance();

            string type = Token.TT_EQ;
            if (CurrentChar == '=')
            {
                Advance();
                type = Token.TT_EE;
            }

            return new Token(type, start: start, end: Position);
        }

        public Token MakeLessThan()
        {
            Position start = Position.Copy();
            Advance();

            string type = Token.TT_LT;
            if (CurrentChar == '=')
            {
                Advance();
                type = Token.TT_LTE;
            }

            return new Token(type, start: start, end: Position);
        }

        public Token MakeGreaterThan()
        {
            Position start = Position.Copy();
            Advance();

            string type = Token.TT_GT;
            if (CurrentChar == '=')
            {
                Advance();
                type = Token.TT_GTE;
            }

            return new Token(type, start: start, end: Position);
        }

    }
}
