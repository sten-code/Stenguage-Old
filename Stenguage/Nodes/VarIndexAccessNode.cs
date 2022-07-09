using Stenguage.Errors;
using Stenguage.Objects;

namespace Stenguage.Nodes
{
    public class VarIndexAccessNode : Node
    {
        public Token ObjectToken;
        public Node IndexNode;

        public VarIndexAccessNode(Token objectToken, Node indexNode)
        {
            ObjectToken = objectToken;
            IndexNode = indexNode;

            Start = objectToken.Start;
            End = indexNode.End;
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();

            Object index = res.Register(IndexNode.Visit(context));
            if (res.ShouldReturn()) return res;

            (Object num, Error error) = context.SymbolTable.Get(ObjectToken.Value).GetIndex(index);
            if (error != null) return res.Failure(error);
            
            return res.Success(num.SetPosition(Start, End));
        }
    }
}
