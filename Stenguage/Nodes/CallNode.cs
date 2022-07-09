using Stenguage.Errors;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;
using System.Linq;

namespace Stenguage.Nodes
{
    public class CallNode : Node
    {
        public Node NodeToCall;
        public List<Node> ArgNodes;

        public CallNode(Node callNode, List<Node> argNodes)
        {
            NodeToCall = callNode;
            ArgNodes = argNodes;

            Start = callNode.Start;

            if (argNodes.Count > 0)
            {
                End = argNodes.Last().End;
            }
            else
            {
                End = callNode.End;
            }
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();
            List<Object> args = new List<Object>();

            BaseFunction callValue = (BaseFunction)res.Register(NodeToCall.Visit(context));
            if (res.ShouldReturn()) return res;
            callValue = (BaseFunction)((BaseFunction)callValue.Clone()).SetPosition(Start, End).SetContext(context);

            foreach (Node argNode in ArgNodes)
            {
                args.Add(res.Register(argNode.Visit(context)));
                if (res.ShouldReturn()) return res;
            }

            for (int i = 0; i < callValue.ArgNames.Count; i++)
            {
                System.Type expectedType = callValue.ArgNames.ElementAt(i).Value.Value;
                System.Type actualType = args[i].GetType();
                if (expectedType != actualType && !actualType.IsSubclassOf(expectedType))
                {
                    return res.Failure(new RuntimeError(Start, End, $"'{callValue.ArgNames.ElementAt(i).Key}' is not type '{actualType.Name}', expected '{expectedType.Name}'", context));
                }
            }

            Object value = res.Register(callValue.Execute(args));
            if (res.ShouldReturn()) return res;
            
            value = ((Object)value.Clone()).SetPosition(Start, End).SetContext(context);
            return res.Success(value);
        }
    }
}
