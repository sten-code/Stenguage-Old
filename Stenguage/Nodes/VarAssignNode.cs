using Stenguage.Objects;

namespace Stenguage.Nodes
{
    public class VarAssignNode : Node
    {
        public Token VarNameToken;
        public Node ValueNode;

        public VarAssignNode(Token varNameToken, Node valueNode)
        {
            VarNameToken = varNameToken;
            ValueNode = valueNode;
            Start = valueNode.Start;
            End = valueNode.End;
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();
            string varName = VarNameToken.Value;
            Object value = res.Register(ValueNode.Visit(context));
            if (res.ShouldReturn()) return res;

            context.SymbolTable.Set(varName, value, false);
            return res.Success(value);
        }
    }
}
