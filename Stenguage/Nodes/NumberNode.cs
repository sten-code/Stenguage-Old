using Stenguage.Objects;

namespace Stenguage.Nodes
{
    public class NumberNode : Node
    {
        public Token Token;

        public NumberNode(Token token)
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
            return new RuntimeResult().Success(new Number(float.Parse(Token.Value)).SetContext(context).SetPosition(Start, End));
        }
    }
}
