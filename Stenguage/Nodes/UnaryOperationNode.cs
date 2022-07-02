using Stenguage.Errors;
using Stenguage.Objects;

namespace Stenguage.Nodes
{
    public class UnaryOpNode : Node
    {
        public Token OperationToken;
        public Node Node;

        public UnaryOpNode(Token opToken, Node node)
        {
            OperationToken = opToken;
            Node = node;
            Start = node.Start;
            End = node.End;
        }

        public override string ToString()
        {
            return $"({OperationToken}, {Node})";
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();
            Object number = res.Register(Node.Visit(context));
            if (res.ShouldReturn()) return res;

            Error error = null;
            if (OperationToken.Type == Token.TT_MINUS)
            {
                (number, error) = number.MultipliedBy(new Number(-1));
            }
            else if (OperationToken.Matches(Token.TT_KEYWORD, "not"))
            {
                (number, error) = number.Not();
            }

            if (error != null) return res.Failure(error);
            else return res.Success(number.SetPosition(Start, End));
        }
    }
}
