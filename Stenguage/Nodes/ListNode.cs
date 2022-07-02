using Stenguage.Objects;
using System.Collections.Generic;

namespace Stenguage.Nodes
{
    public class ListNode : Node
    {
        public List<Node> ElementNodes;

        public ListNode(List<Node> elementNodes, Position start, Position end)
        {
            ElementNodes = elementNodes;
            Start = start;
            End = end;
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();
            List<Object> elements = new List<Object>();

            foreach (Node elementNode in ElementNodes)
            {
                elements.Add(res.Register(elementNode.Visit(context)));
                if (res.ShouldReturn()) return res;
            }

            return res.Success(new List(elements).SetContext(context).SetPosition(Start, End));
        }
    }
}
