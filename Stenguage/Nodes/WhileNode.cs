using Stenguage.Objects;
using System.Collections.Generic;

namespace Stenguage.Nodes
{
    public class WhileNode : Node
    {
        public Node ConditionNode;
        public Node BodyNode;
        public bool ReturnNull;

        public WhileNode(Node conditionNode, Node bodyNode, bool returnNull)
        {
            ConditionNode = conditionNode;
            BodyNode = bodyNode;
            ReturnNull = returnNull;

            Start = conditionNode.Start;
            End = bodyNode.End;
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();
            List<Object> elements = new List<Object>();

            while (true)
            {
                Boolean condition = (Boolean)res.Register(ConditionNode.Visit(context));
                if (res.ShouldReturn()) return res;

                if (!condition.Value) break;

                Object value = res.Register(BodyNode.Visit(context));
                if (res.ShouldReturn() && !res.LoopContinue && !res.LoopBreak) return res;
                if (res.LoopContinue) continue;
                if (res.LoopBreak) break;
                elements.Add(value);
            }

            return res.Success(ReturnNull ? Number.Null : new List(elements).SetContext(context).SetPosition(Start, End));
        }
    }
}
