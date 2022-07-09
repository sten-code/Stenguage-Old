using Stenguage.Errors;
using Stenguage.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stenguage.Parsing
{
    public class Parser
    {
        public List<Token> Tokens;
        public int TokenIndex;
        public Token CurrentToken;

        public Parser(List<Token> tokens)
        {
            Tokens = tokens;
            TokenIndex = -1;
            Advance();
        }

        public Token Advance(int amount=1)
        {
            TokenIndex += amount;
            UpdateCurrentToken();
            return CurrentToken;
        }

        public Token Reverse(int amount=1)
        {
            TokenIndex -= amount;
            UpdateCurrentToken();
            return CurrentToken;
        }

        public void UpdateCurrentToken()
        {
            if (TokenIndex >= 0 && TokenIndex < Tokens.Count)
            {
                CurrentToken = Tokens[TokenIndex];
            }
        }

        public ParseResult Parse()
        {
            ParseResult res = Statements();
            if (res.Error == null && CurrentToken.Type != Token.TT_EOF)
            {
                return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected '+', '-', '*', '/', '^', '==', '!=', '<', '>', <=', '>=', 'and' or 'or'"));
            }
            return res;
        }

        public ParseResult Statements()
        {
            ParseResult res = new ParseResult();
            List<Node> statements = new List<Node>();
            Position start = CurrentToken.Start.Copy();

            while (CurrentToken.Type == Token.TT_NEWLINE)
            {
                res.RegisterAdvancement();
                Advance();
            }

            Node statement = (Node)res.Register(Statement());
            if (res.Error != null) return res;
            statements.Add(statement);

            bool moreStatements = true;

            while (true)
            {
                int newlineCount = 0;
                while (CurrentToken.Type == Token.TT_NEWLINE)
                {
                    res.RegisterAdvancement();
                    Advance();
                    newlineCount++;
                }
                if (newlineCount == 0)
                {
                    moreStatements = false;
                }

                if (!moreStatements) break;
                statement = (Node)res.TryRegister(Statement());
                if (statement == null)
                {
                    Reverse(res.ToReverseCount);
                    moreStatements = false;
                    continue;
                }
                statements.Add(statement);
            }

            return res.Success(new ListNode(statements, start, CurrentToken.End.Copy()));
        }

        public ParseResult Statement()
        {
            ParseResult res = new ParseResult();
            Position start = CurrentToken.Start.Copy();

            if (CurrentToken.Matches(Token.TT_KEYWORD, "return"))
            {
                res.RegisterAdvancement();
                Advance();

                Node returnExpr = (Node)res.TryRegister(Expr());
                if (returnExpr == null) Reverse(res.ToReverseCount);
                return res.Success(new ReturnNode(returnExpr, start, CurrentToken.Start.Copy()));
            }

            if (CurrentToken.Matches(Token.TT_KEYWORD, "continue"))
            {
                res.RegisterAdvancement();
                Advance();
                return res.Success(new ContinueNode(start, CurrentToken.Start.Copy()));
            }

            if (CurrentToken.Matches(Token.TT_KEYWORD, "break"))
            {
                res.RegisterAdvancement();
                Advance();
                return res.Success(new BreakNode(start, CurrentToken.Start.Copy()));
            }

            Node expr = (Node)res.Register(Expr());
            if (res.Error != null) return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected 'return', 'continue', 'break', 'var', 'if', 'for', 'while', 'func', int, float, identifier, '+', '-', '(', '[' or 'not'"));

            return res.Success(expr);
        }

        public ParseResult Call()
        {
            ParseResult res = new ParseResult();
            Node atom = (Node)res.Register(Atom());
            if (res.Error != null) return res;

            if (CurrentToken.Type == Token.TT_LPAREN)
            {
                res.RegisterAdvancement();
                Advance();
                List<Node> argNodes = new List<Node>();

                if (CurrentToken.Type == Token.TT_RPAREN)
                {
                    res.RegisterAdvancement();
                    Advance();
                }
                else
                {
                    argNodes.Add((Node)res.Register(Expr()));
                    if (res.Error != null) return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected ')', 'var', 'if', 'for', 'while', 'func', int, float, identifier, '+', '-', '(', '[' or 'not"));
                    while (CurrentToken.Type == Token.TT_COMMA)
                    {
                        res.RegisterAdvancement();
                        Advance();

                        argNodes.Add((Node)res.Register(Expr()));
                        if (res.Error != null) return res;
                    }

                    if (CurrentToken.Type != Token.TT_RPAREN)
                    {
                        return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected ',' or ')'"));
                    }

                    res.RegisterAdvancement();
                    Advance();
                }
                return res.Success(new CallNode(atom, argNodes));
            }
            return res.Success(atom);
        }

        public ParseResult Atom()
        {
            ParseResult res = new ParseResult();
            Token token = CurrentToken;
            if (new string[] { Token.TT_INT, Token.TT_FLOAT }.Contains(token.Type))
            {
                res.RegisterAdvancement();
                Advance();
                return res.Success(new NumberNode(token));
            }
            else if (token.Type == Token.TT_STRING)
            {
                res.RegisterAdvancement();
                Advance();
                return res.Success(new StringNode(token));
            }
            else if (token.Type == Token.TT_IDENTIFIER)
            {
                res.RegisterAdvancement();
                Advance();
                if (CurrentToken.Type == Token.TT_LBRACK)
                {
                    res.RegisterAdvancement();
                    Advance();
                    Node indexNode = (Node)res.Register(Expr());
                    if (CurrentToken.Type != Token.TT_RBRACK)
                    {
                        return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected ']'"));
                    }
                    res.RegisterAdvancement();
                    Advance();
                    return res.Success(new VarIndexAccessNode(token, indexNode));
                }
                else
                {
                    return res.Success(new VarAccessNode(token));
                }
            }
            else if (token.Type == Token.TT_LPAREN)
            {
                res.RegisterAdvancement();
                Advance();
                Node expr = (Node)res.Register(Expr());
                if (res.Error != null) return res;
                if (CurrentToken.Type == Token.TT_RPAREN)
                {
                    res.RegisterAdvancement();
                    Advance();
                    return res.Success(expr);
                }
                else
                {
                    return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected ')'"));
                }
            }
            else if (token.Type == Token.TT_LBRACK)
            {
                Node listExpr = (Node)res.Register(ListExpr());
                if (res.Error != null) return res;
                return res.Success(listExpr);
            }
            else if (token.Matches(Token.TT_KEYWORD, "if"))
            {
                Node ifExpr = (Node)res.Register(IfExpr());
                if (res.Error != null) return res;
                return res.Success(ifExpr);
            }
            else if (token.Matches(Token.TT_KEYWORD, "for"))
            {
                Node forExpr = (Node)res.Register(ForExpr());
                if (res.Error != null) return res;
                return res.Success(forExpr);
            }
            else if (token.Matches(Token.TT_KEYWORD, "while"))
            {
                Node whileExpr = (Node)res.Register(WhileExpr());
                if (res.Error != null) return res;
                return res.Success(whileExpr);
            }
            else if (token.Matches(Token.TT_KEYWORD, "func"))
            {
                Node funcDef = (Node)res.Register(FuncDef());
                if (res.Error != null) return res;
                return res.Success(funcDef);
            }
            return res.Failure(new InvalidSyntaxError(token.Start, token.End, "Expected int, float, identifier, '+', '-', '(', '[', 'if', 'for', 'while' or 'func'"));
        }

        private ParseResult ListExpr()
        {
            ParseResult res = new ParseResult();
            List<Node> elementNodes = new List<Node>();
            Position start = CurrentToken.Start.Copy();

            if (CurrentToken.Type != Token.TT_LBRACK)
            {
                return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected '['"));
            }

            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type == Token.TT_RBRACK)
            {
                res.RegisterAdvancement();
                Advance();
            }
            else
            {
                elementNodes.Add((Node)res.Register(Expr()));
                if (res.Error != null) return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected ']', 'var', 'if', 'for', 'while', 'func', int, float, identifier, '+', '-', '(', '[' or 'not"));
                while (CurrentToken.Type == Token.TT_COMMA)
                {
                    res.RegisterAdvancement();
                    Advance();

                    elementNodes.Add((Node)res.Register(Expr()));
                    if (res.Error != null) return res;
                }

                if (CurrentToken.Type != Token.TT_RBRACK)
                {
                    return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected ',' or ']'"));
                }

                res.RegisterAdvancement();
                Advance();
            }

            return res.Success(new ListNode(elementNodes, start, CurrentToken.End.Copy()));
        }

        public ParseResult FuncDef()
        {
            ParseResult res = new ParseResult();

            if (!CurrentToken.Matches(Token.TT_KEYWORD, "func"))
            {
                return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected 'func'"));
            }

            res.RegisterAdvancement();
            Advance();

            Token varNameToken = null;
            if (CurrentToken.Type == Token.TT_IDENTIFIER)
            {
                varNameToken = CurrentToken;
                res.RegisterAdvancement();
                Advance();
                if (CurrentToken.Type != Token.TT_LPAREN)
                {
                    return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected '('"));
                }
            }
            else
            {
                if (CurrentToken.Type != Token.TT_LPAREN)
                {
                    return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected identifier or '('"));
                }
            }

            res.RegisterAdvancement();
            Advance();
            List<Token> argNameTokens = new List<Token>();

            if (CurrentToken.Type == Token.TT_IDENTIFIER)
            {
                argNameTokens.Add(CurrentToken);
                res.RegisterAdvancement();
                Advance();
                while (CurrentToken.Type == Token.TT_COMMA)
                {
                    res.RegisterAdvancement();
                    Advance();
                    if (CurrentToken.Type != Token.TT_IDENTIFIER)
                    {
                        return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected identifier"));
                    }

                    argNameTokens.Add(CurrentToken);
                    res.RegisterAdvancement();
                    Advance();
                }

                if (CurrentToken.Type != Token.TT_RPAREN)
                {
                    return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected ',' or ')'"));
                }
            }
            else
            {
                if (CurrentToken.Type != Token.TT_RPAREN)
                {
                    return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected identifier or ')'"));
                }
            }

            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type == Token.TT_ARROW)
            {
                res.RegisterAdvancement();
                Advance();

                Node node = (Node)res.Register(Expr());
                if (res.Error != null) return res;

                return res.Success(new FunctionNode(varNameToken, argNameTokens, node, true));
            }

            if (CurrentToken.Type != Token.TT_NEWLINE)
            {
                return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected '->' or newline"));
            }

            res.RegisterAdvancement();
            Advance();

            Node body = (Node)res.Register(Statements());
            if (res.Error != null) return res;

            if (!CurrentToken.Matches(Token.TT_KEYWORD, "end"))
            {
                return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected 'end'"));
            }

            res.RegisterAdvancement();
            Advance();

            return res.Success(new FunctionNode(varNameToken, argNameTokens, body, false));


        }

        public ParseResult ForExpr()
        {
            ParseResult res = new ParseResult();

            if (!CurrentToken.Matches(Token.TT_KEYWORD, "for"))
            {
                return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected 'for'"));
            }

            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type != Token.TT_IDENTIFIER)
            {
                return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected identifier"));
            }

            Token varName = CurrentToken;
            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type != Token.TT_EQ)
            {
                return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected '='"));
            }

            res.RegisterAdvancement();
            Advance();

            Node startValue = (Node)res.Register(Expr());
            if (res.Error != null) return res;

            if (!CurrentToken.Matches(Token.TT_KEYWORD, "to"))
            {
                return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected 'to'"));
            }

            res.RegisterAdvancement();
            Advance();

            Node endValue = (Node)res.Register(Expr());
            if (res.Error != null) return res;
            Node stepValue = null;
            if (CurrentToken.Matches(Token.TT_KEYWORD, "step"))
            {
                res.RegisterAdvancement();
                Advance();

                stepValue = (Node)res.Register(Expr());
                if (res.Error != null) return res;
            }

            if (!CurrentToken.Matches(Token.TT_KEYWORD, "then"))
            {
                return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected 'then' or 'step'"));
            }

            res.RegisterAdvancement();
            Advance();

            Node body;
            if (CurrentToken.Type == Token.TT_NEWLINE)
            {
                res.RegisterAdvancement();
                Advance();

                body = (Node)res.Register(Statements());
                if (res.Error != null) return res;

                if (!CurrentToken.Matches(Token.TT_KEYWORD, "end"))
                {
                    return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected 'end'"));
                }

                res.RegisterAdvancement();
                Advance();

                return res.Success(new ForNode(varName, startValue, endValue, stepValue, body, true));
            }

            body = (Node)res.Register(Statement());
            if (res.Error != null) return res;
            return res.Success(new ForNode(varName, startValue, endValue, stepValue, body, false));
        }

        public ParseResult WhileExpr()
        {
            ParseResult res = new ParseResult();

            if (!CurrentToken.Matches(Token.TT_KEYWORD, "while"))
            {
                return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected 'while'"));
            }

            res.RegisterAdvancement();
            Advance();

            Node condition = (Node)res.Register(Expr());
            if (res.Error != null) return res;

            if (!CurrentToken.Matches(Token.TT_KEYWORD, "then"))
            {
                return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected 'then'"));
            }

            res.RegisterAdvancement();
            Advance();

            Node body;
            if (CurrentToken.Type == Token.TT_NEWLINE)
            {
                res.RegisterAdvancement();
                Advance();

                body = (Node)res.Register(Statements());
                if (res.Error != null) return res;

                if (!CurrentToken.Matches(Token.TT_KEYWORD, "end"))
                {
                    return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected 'end'"));
                }

                res.RegisterAdvancement();
                Advance();

                return res.Success(new WhileNode(condition, body, true));
            }

            body = (Node)res.Register(Statement());
            if (res.Error != null) return res;

            return res.Success(new WhileNode(condition, body, false));
        }

        public ParseResult IfExpr()
        {
            ParseResult res = new ParseResult();
            (List<(Node, Node, bool)>, (Node, bool)) allCases = ((List<(Node, Node, bool)>, (Node, bool)))res.Register(IfExprCases("if"));
            if (res.Error != null) return res;

            (List<(Node, Node, bool)> cases, (Node, bool) elseCase) = allCases;
            return res.Success(new IfNode(cases, elseCase));
        }

        public ParseResult IfExprB()
        {
            return IfExprCases("elif");
        }

        public ParseResult IfExprC()
        {
            ParseResult res = new ParseResult();
            (Node, bool) elseCase = (null, false);

            if (CurrentToken.Matches(Token.TT_KEYWORD, "else"))
            {
                res.RegisterAdvancement();
                Advance();

                if (CurrentToken.Type == Token.TT_NEWLINE)
                {
                    Node statements = (Node)res.Register(Statements());
                    if (res.Error != null) return res;
                    elseCase = (statements, true);

                    if (CurrentToken.Matches(Token.TT_KEYWORD, "end"))
                    {
                        res.RegisterAdvancement();
                        Advance();
                    }
                    else
                    {
                        return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected 'end'"));
                    }
                } else
                {
                    Node expr = (Node)res.Register(Statement());
                    if (res.Error != null) return res;
                    elseCase = (expr, false);

                }
            }
            return res.Success(elseCase);
        }

        public ParseResult IfExprBOrC()
        {
            ParseResult res = new ParseResult();
            List<(Node, Node, bool)> cases = new List<(Node, Node, bool)>();
            (Node, bool) elseCase = (null, false);

            if (CurrentToken.Matches(Token.TT_KEYWORD, "elif"))
            {
                (List<(Node, Node, bool)>, (Node, bool)) allCases = ((List<(Node, Node, bool)>, (Node, bool)))res.Register(IfExprB());
                if (res.Error != null) return res;
                (cases, elseCase) = allCases;
            } else
            {
                elseCase = ((Node, bool))res.Register(IfExprC());
                if (res.Error != null) return res;
            }

            return res.Success((cases, elseCase));
        }

        public ParseResult IfExprCases(string caseKeyword)
        {
            ParseResult res = new ParseResult();
            List<(Node, Node, bool)> cases = new List<(Node, Node, bool)>();
            (Node, bool) elseCase = (null, false);

            if (!CurrentToken.Matches(Token.TT_KEYWORD, caseKeyword))
            {
                return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, $"Expected '{caseKeyword}'"));
            }

            res.RegisterAdvancement();
            Advance();

            Node condition = (Node)res.Register(Expr());
            if (res.Error != null) return res;

            if (!CurrentToken.Matches(Token.TT_KEYWORD, "then"))
            {
                return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected 'then'"));
            }

            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type == Token.TT_NEWLINE)
            {
                res.RegisterAdvancement();
                Advance();

                Node statements = (Node)res.Register(Statements());
                if (res.Error != null) return res;
                cases.Add((condition, statements, true));

                if (CurrentToken.Matches(Token.TT_KEYWORD, "end"))
                {
                    res.RegisterAdvancement();
                    Advance();
                } else
                {
                    (List<(Node, Node, bool)>, (Node, bool)) allCases = ((List<(Node, Node, bool)>, (Node, bool)))res.Register(IfExprBOrC());
                    if (res.Error != null) return res;

                    elseCase = allCases.Item2;
                    cases.AddRange(allCases.Item1);
                }
            } else
            {
                Node expr = (Node)res.Register(Statement());
                if (res.Error != null) return res;
                cases.Add((condition, expr, false));

                (List<(Node, Node, bool)>, (Node, bool)) allCases = ((List<(Node, Node, bool)>, (Node, bool)))res.Register(IfExprBOrC());
                if (res.Error != null) return res;

                elseCase = allCases.Item2;
                cases.AddRange(allCases.Item1);
            }

            return res.Success((cases, elseCase));
        }

        public ParseResult Power()
        {
            return BinaryOperation(Call, new (string, string)[] { (Token.TT_POW, "") }, Factor);
        }

        public ParseResult Factor()
        {
            ParseResult res = new ParseResult();
            Token token = CurrentToken;

            if (new string[] { Token.TT_PLUS, Token.TT_MINUS }.Contains(token.Type))
            {
                res.RegisterAdvancement();
                Advance();
                Node factor = (Node)res.Register(Factor());
                if (res.Error != null) return res;
                return res.Success(new UnaryOpNode(token, factor));
            }
            Node power = (Node)res.Register(Power());
            if (res.Error != null) return res;
            return res.Success(power);
        }

        public ParseResult Term()
        {
            return BinaryOperation(Factor, new (string, string)[] { (Token.TT_MUL, ""), (Token.TT_DIV, "") });
        }

        public ParseResult CompExpr()
        {
            ParseResult res = new ParseResult();
            Token token;

            Node node;
            if (CurrentToken.Matches(Token.TT_KEYWORD, "not"))
            {
                token = CurrentToken;
                res.RegisterAdvancement();
                Advance();

                node = (Node)res.Register(CompExpr());
                if (res.Error != null) return res;
                else res.Success(new UnaryOpNode(token, node));
            }

            node = (Node)res.Register(BinaryOperation(ArithExpr, new (string, string)[] { (Token.TT_EE, ""), (Token.TT_NE, ""), (Token.TT_LT, ""), (Token.TT_GT, ""), (Token.TT_LTE, ""), (Token.TT_GTE, "") }));
            if (res.Error != null) return res;
            return res.Success(node);
        }

        public ParseResult ArithExpr()
        {
            return BinaryOperation(Term, new (string, string)[] { (Token.TT_PLUS, ""), (Token.TT_MINUS, "") });
        }

        public ParseResult Expr()
        {
            ParseResult res = new ParseResult();
            if (CurrentToken.Matches(Token.TT_KEYWORD, "var"))
            {
                res.RegisterAdvancement();
                Advance();
                if (CurrentToken.Type != Token.TT_IDENTIFIER)
                {
                    return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected identifier"));
                }

                Token varName = CurrentToken;
                res.RegisterAdvancement();
                Advance();

                if (CurrentToken.Type == Token.TT_EQ)
                {
                    res.RegisterAdvancement();
                    Advance();
                    Node expr = (Node)res.Register(Expr());
                    if (res.Error != null) return res;
                    return res.Success(new VarAssignNode(varName, expr));
                }
                else if (CurrentToken.Type == Token.TT_LBRACK)
                {
                    res.RegisterAdvancement();
                    Advance();

                    Node indexNode = (Node)res.Register(Expr());
                    if (res.Error != null) return res;

                    if (CurrentToken.Type != Token.TT_RBRACK)
                    {
                        return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected ']'"));
                    }

                    res.RegisterAdvancement();
                    Advance();

                    if (CurrentToken.Type != Token.TT_EQ)
                    {
                        return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected '='"));
                    }

                    res.RegisterAdvancement();
                    Advance();

                    Node expr = (Node)res.Register(Expr());
                    if (res.Error != null) return res;

                    return res.Success(new VarIndexAssignNode(varName, indexNode, expr));
                }
                else
                {
                    return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected '=' or '['"));
                }
            }
            if (CurrentToken.Type == Token.TT_IDENTIFIER)
            {
                Token varName = CurrentToken;
                res.RegisterAdvancement();
                Advance();
                if (new string[] { Token.TT_PLUS, Token.TT_MINUS, Token.TT_MUL, Token.TT_DIV }.Contains(CurrentToken.Type))
                {
                    Token token = CurrentToken;
                    res.RegisterAdvancement();
                    Advance();

                    if (CurrentToken.Type == Token.TT_EQ)
                    {
                        res.RegisterAdvancement();
                        Advance();

                        Node expr = (Node)res.Register(Expr());
                        if (res.Error != null) return res.Failure(new InvalidSyntaxError(CurrentToken.Start, CurrentToken.End, "Expected 'var', 'if', 'for', 'while', 'func', int, float, identifier, '+', '-', '(', '[' or 'not'"));

                        return res.Success(new VarAssignNode(varName, expr, token));
                    }
                    Reverse();
                }
                Reverse();
            }

            Node node = (Node)res.Register(BinaryOperation(CompExpr, new (string, string)[] { (Token.TT_KEYWORD, "and"), (Token.TT_KEYWORD, "or") }));
            if (res.Error != null) return res;

            return res.Success(node);
        }

        public ParseResult BinaryOperation(Func<ParseResult> funca, (string, string)[] ops, Func<ParseResult> funcb = null)
        {
            if (funcb == null)
            {
                funcb = funca;
            }

            ParseResult res = new ParseResult();
            Node left = (Node)res.Register(funca());
            if (res.Error != null) return res;

            while (ops.Contains((CurrentToken.Type, "")) || ops.Contains((CurrentToken.Type, CurrentToken.Value)))
            {
                Token opToken = CurrentToken;
                res.RegisterAdvancement();
                Advance();
                Node right = (Node)res.Register(funcb());
                if (res.Error != null) return res;
                left = new BinaryOperationNode(left, opToken, right);
            }

            return res.Success(left);
        }
    }

}
