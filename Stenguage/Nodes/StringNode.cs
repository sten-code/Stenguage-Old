using Stenguage.Objects;

namespace Stenguage.Nodes
{
    public class StringNode : Node
    {
        public Token Token;

        public StringNode(Token token)
        {
            Token = token;
            Start = token.Start;
            End = token.End;
        }

        public override string ToString()
        {
            return $"{Token}";
        }

        public override RuntimeResult Visit(Context context)
        {
            return new RuntimeResult().Success(new String(Token.Value).SetContext(context).SetPosition(Start, End));
        }
    }
}
