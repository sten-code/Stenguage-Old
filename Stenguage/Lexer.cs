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

        public string Name;

        public Lexer(string name, string text)
        {
            Name = name;
            Text = text;
            Position = new Position(-1, 0, -1, name, text);
            CurrentChar = char.MaxValue;
            Advance();
        }

        // Advances to the next character
        public void Advance()
        {
            Position.Advance(CurrentChar);
            CurrentChar = Position.Index < Text.Length ? Text[Position.Index] : char.MaxValue;
        }

        // Converts the text into a list of tokens which the interpreter can read
        public (List<Token>, Error) MakeTokens()
        {
            List<Token> tokens = new List<Token>();

            while (CurrentChar != char.MaxValue)
            {
                if (" \t".Contains(CurrentChar))
                {
                    Advance();
                }
                else if (CurrentChar == '#') 
                {
                    // Create comment
                    Advance();
                    // Finds the end of the current line
                    while (CurrentChar != '\n' && CurrentChar != char.MaxValue) Advance();
                    Advance();
                }
                else if (CurrentChar.Equals(';') || CurrentChar.Equals('\n') || CurrentChar == 13)
                {
                    // Create newline token
                    tokens.Add(new Token(Token.TT_NEWLINE, start: Position));
                    Advance();
                }
                else if ("0123456789.".Contains(CurrentChar))
                {
                    // Create number token
                    string numStr = "";
                    int dotCount = 0;
                    Position start = Position.Copy();

                    // If the number begins with a . it means its a float, this allows you to write floats as .2 instead of 0.2
                    if (CurrentChar == '.')
                    {
                        // Converts the .2 into a 0.2
                        dotCount = 1;
                        numStr = "0.";
                        Advance();
                        // Finds the end of the number
                        while (CurrentChar != char.MaxValue && "0123456789".Contains(CurrentChar))
                        {
                            numStr += CurrentChar;
                            Advance();
                        }
                    } else
                    {
                        // Finds the end of the number
                        while (CurrentChar != char.MaxValue && "0123456789.".Contains(CurrentChar))
                        {
                            if (CurrentChar == '.')
                            {
                                // A number can have maximum 1 . inside it, if it encounters another one it will just mark it as the end of the number and leave the . to the next cycle to give it an error.
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
                    }

                    // If the number contains a . it means its a float, if not its a full number which is an integer (int)
                    if (dotCount == 0) tokens.Add(new Token(Token.TT_INT, numStr, start: start, end: Position));
                    else tokens.Add(new Token(Token.TT_FLOAT, numStr, start: start, end: Position));
                }
                else if ("0123456789abcdefghijklnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_".Contains(CurrentChar))
                {
                    // Create identifier token
                    string idStr = "";
                    Position start = Position.Copy();

                    // Repeats until it has found the end of the identifier
                    while (CurrentChar != char.MaxValue && "0123456789abcdefghijklnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_.".Contains(CurrentChar))
                    {
                        idStr += CurrentChar;
                        Advance();
                    }

                    tokens.Add(new Token(Token.KEYWORDS.Contains(idStr) ? Token.TT_KEYWORD : Token.TT_IDENTIFIER, idStr, start, Position));
                }
                else if (CurrentChar == '"')
                {
                    // Create string token
                    string str = "";
                    Position start = Position.Copy();
                    bool escape = false;
                    Advance();

                    Dictionary<char, char> escapeCharacters = new Dictionary<char, char>
                    {
                        ['n'] = '\n',
                        ['t'] = '\t',
                    };

                    // Repeats until the end of the string is found
                    while (CurrentChar != char.MaxValue && (CurrentChar != '"' || escape))
                    {
                        if (escape)
                        {
                            // If the character after a \ is inside the escape characters dictionary it can be added as a special character, if its not inside it will just be added as a normal character
                            // A " can be added with \" and a \ can be added with \\
                            str += escapeCharacters.ContainsKey(CurrentChar) ? escapeCharacters[CurrentChar] : CurrentChar;
                        }
                        else
                        {
                            // If the current character is a \ it means the next character is a special character such as a new line.
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
                    tokens.Add(new Token(Token.TT_STRING, str, start: start, end: Position));
                }
                else if (CurrentChar == '+')
                {
                    // Create plus token
                    tokens.Add(new Token(Token.TT_PLUS, start: Position));
                    Advance();
                }
                else if (CurrentChar == '-')
                {
                    // Create minus or arrow token
                    string tokenType = Token.TT_MINUS;
                    Position start = Position.Copy();
                    Advance();

                    // If the next character is a > it means the entire thing is a -> which means its an arrow token
                    if (CurrentChar == '>')
                    {
                        Advance();
                        tokenType = Token.TT_ARROW;
                    }

                    tokens.Add(new Token(tokenType, start: start, end: Position));
                }
                else if (CurrentChar == '*')
                {
                    // Create multiply token
                    tokens.Add(new Token(Token.TT_MUL, start: Position));
                    Advance();
                }
                else if (CurrentChar == '/')
                {
                    // Create division token
                    tokens.Add(new Token(Token.TT_DIV, start: Position));
                    Advance();
                }
                else if (CurrentChar == '^')
                {
                    // Create caret token
                    tokens.Add(new Token(Token.TT_POW, start: Position));
                    Advance();
                }
                else if (CurrentChar == '(')
                {
                    // Create lparen token
                    tokens.Add(new Token(Token.TT_LPAREN, start: Position));
                    Advance();
                }
                else if (CurrentChar == ')')
                {
                    // Create rparen token
                    tokens.Add(new Token(Token.TT_RPAREN, start: Position));
                    Advance();
                }
                else if (CurrentChar == '[')
                {
                    // Create lbrack token
                    tokens.Add(new Token(Token.TT_LBRACK, start: Position));
                    Advance();
                }
                else if (CurrentChar == ']')
                {
                    // Create rbrack token
                    tokens.Add(new Token(Token.TT_RBRACK, start: Position));
                    Advance();
                }
                else if (CurrentChar == '!')
                {
                    // Create not equals token
                    Position start = Position.Copy();
                    Advance();

                    // If the character after an ! is an = it means the whole thing is a != token which means its a not equals token
                    if (CurrentChar == '=')
                    {
                        Advance();
                        tokens.Add(new Token(Token.TT_NE, start: start, end: Position));
                    } else
                    {
                        Advance();
                        return (new List<Token>(), new ExpectedCharError(start, Position, "Expected '=' (after '!')"));
                    }
                }
                else if (CurrentChar == '=')
                {
                    // Create equals token
                    Position start = Position.Copy();
                    Advance();

                    // If there are 2 = signs it means its an equals equals for comparisons, if not its a normal equals for assignment
                    string type = Token.TT_EQ;
                    if (CurrentChar == '=')
                    {
                        Advance();
                        type = Token.TT_EE;
                    }

                    tokens.Add(new Token(type, start: start, end: Position));
                }
                else if (CurrentChar == '<')
                {
                    // Create less than or equal to token
                    Position start = Position.Copy();
                    Advance();

                    // If the character after a < is an equals it means its a less than or equals to token, if not its a normal less than token
                    string type = Token.TT_LT;
                    if (CurrentChar == '=')
                    {
                        Advance();
                        type = Token.TT_LTE;
                    }

                    tokens.Add(new Token(type, start: start, end: Position));
                }
                else if (CurrentChar == '>')
                {
                    // Create greater than or equal to token
                    Position start = Position.Copy();
                    Advance();

                    // If the character after a > is an equals it means its a greater than or equals to token, if not its a normal greater than token
                    string type = Token.TT_GT;
                    if (CurrentChar == '=')
                    {
                        Advance();
                        type = Token.TT_GTE;
                    }

                    tokens.Add(new Token(type, start: start, end: Position));
                }
                else if (CurrentChar == ',')
                {
                    // Create comma token
                    tokens.Add(new Token(Token.TT_COMMA, start: Position));
                    Advance();
                }
                else
                {
                    // Token not found which means it's an illegal character
                    Position start = Position.Copy();
                    return (new List<Token>(), new IllegalCharError(start, Position, $"'{CurrentChar}' ascii: '{(int)CurrentChar}'"));
                }
            }

            tokens.Add(new Token(Token.TT_EOF, start: Position));
            return (tokens, null);
        }
    }
}
