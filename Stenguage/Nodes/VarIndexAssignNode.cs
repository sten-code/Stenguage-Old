using Stenguage.Errors;
using Stenguage.Objects;

namespace Stenguage.Nodes
{
    public class VarIndexAssignNode : Node
    {
        public Token VarNameToken;
        public Node IndexNode;
        public Node ValueNode;

        public VarIndexAssignNode(Token varName, Node indexNode, Node valueNode)
        {
            VarNameToken = varName;
            IndexNode = indexNode;
            ValueNode = valueNode;

            Start = varName.Start;
            End = valueNode.End;
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();

            Object value = res.Register(ValueNode.Visit(context));
            if (res.ShouldReturn()) return res;

            Object index = res.Register(IndexNode.Visit(context));
            if (res.ShouldReturn()) return res;

            Object var = context.SymbolTable.Get(VarNameToken.Value);
            (Object newValue, Error error) = var.SetIndex(index, value);
            if (error != null) return res.Failure(error);

            context.SymbolTable.Set(VarNameToken.Value, newValue);
            return res.Success(Number.Null);
        }
    }
}
