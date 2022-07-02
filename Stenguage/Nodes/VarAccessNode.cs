using Stenguage.Errors;
using Stenguage.Objects;

namespace Stenguage.Nodes
{
    public class VarAccessNode : Node
    {
        public Token VarNameToken;

        public VarAccessNode(Token varNameToken)
        {
            VarNameToken = varNameToken;
            Start = VarNameToken.Start;
            End = VarNameToken.End;
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();
            string varName = VarNameToken.Value;
            Object value = context.SymbolTable.Get(varName);

            if (value == null)
            {
                return res.Failure(new RuntimeError(Start, End, $"'{varName}' is not defined", context));
            }

            return res.Success(value);
        }
    }
}
