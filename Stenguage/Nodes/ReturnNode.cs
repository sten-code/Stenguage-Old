using Stenguage.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Stenguage.Nodes
{
    public class ReturnNode : Node
    {
        public Node NodeToReturn;
        public List<Node> ArgNodes;

        public ReturnNode(Node nodeToReturn, Position start, Position end)
        {
            NodeToReturn = nodeToReturn;

            Start = start;
            End = end;
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();

            Object value = Number.Null;
            if (NodeToReturn != null)
            {
                value = res.Register(NodeToReturn.Visit(context));
                if (res.ShouldReturn()) return res;
            }

            return res.SuccessReturn(value);
        }
    }
}
