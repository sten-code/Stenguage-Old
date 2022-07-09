using Stenguage.Errors;
using Stenguage.Objects;

namespace Stenguage.Nodes
{
    public class VarAssignNode : Node
    {
        public Token VarNameToken;
        public Node ValueNode;
        public Token Operation;

        public VarAssignNode(Token varNameToken, Node valueNode, Token operation=null)
        {
            VarNameToken = varNameToken;
            ValueNode = valueNode;
            Operation = operation;

            Start = valueNode.Start;
            End = valueNode.End;
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();
            string varName = VarNameToken.Value;
            Object value = res.Register(ValueNode.Visit(context));
            if (res.ShouldReturn()) return res;

            Object newValue = Number.Null;
            if (Operation != null)
            {
                if (Operation.Type == Token.TT_PLUS)
                {
                    (Object v, Error error) = context.SymbolTable.Get(varName).AddedTo(value);
                    if (error != null) return res.Failure(error);
                    newValue = v;
                }
                else if (Operation.Type == Token.TT_MINUS)
                {
                    (Object v, Error error) = context.SymbolTable.Get(varName).SubtractedFrom(value);
                    if (error != null) return res.Failure(error);
                    newValue = v;
                }
                else if (Operation.Type == Token.TT_MUL)
                {
                    (Object v, Error error) = context.SymbolTable.Get(varName).MultipliedBy(value);
                    if (error != null) return res.Failure(error);
                    newValue = v;
                }
                else if (Operation.Type == Token.TT_DIV)
                {
                    (Object v, Error error) = context.SymbolTable.Get(varName).DividedBy(value);
                    if (error != null) return res.Failure(error);
                    newValue = v;
                }
            } 
            else
            {
                newValue = value;
            }

            context.SymbolTable.Set(varName, newValue, false);
            return res.Success(value);
        }
    }
}
