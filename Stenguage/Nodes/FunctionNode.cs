using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;
using System.Linq;

namespace Stenguage.Nodes
{
    public class FunctionNode : Node
    {
        public Token VarNameToken;
        public List<Token> ArgNameTokens;
        public Node BodyNode;
        public bool AutoReturn;

        public FunctionNode(Token varNameToken, List<Token> argNameTokens, Node body, bool autoReturn)
        {
            VarNameToken = varNameToken;
            ArgNameTokens = argNameTokens;
            BodyNode = body;
            AutoReturn = autoReturn;

            if (varNameToken != null)
            {
                Start = varNameToken.Start;
            }
            else if (argNameTokens.Count > 0)
            {
                Start = argNameTokens[0].Start;
            }
            else
            {
                Start = body.Start;
            }

            End = body.End;
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();
            string funcName = VarNameToken != null ? VarNameToken.Value : null;

            Dictionary<string, ObjectType> argNames = new Dictionary<string, ObjectType>();
            foreach (Token token in ArgNameTokens)
            {
                argNames.Add(token.Value, new ObjectType(typeof(Object)));
            }

            Function funcValue = (Function)new Function(funcName, BodyNode, argNames, AutoReturn).SetContext(context).SetPosition(Start, End);

            if (VarNameToken != null) context.SymbolTable.Set(funcName, funcValue, false);
            return res.Success(funcValue);
        }
    }
}
