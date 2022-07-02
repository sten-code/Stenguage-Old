using Stenguage.Objects;
using System.Collections.Generic;

namespace Stenguage.Nodes
{
    public class ForNode : Node
    {
        public Token VarNameToken;
        public Node StartValueNode;
        public Node EndValueNode;
        public Node StepValueNode;
        public Node BodyNode;
        public bool ReturnNull;

        public ForNode(Token varNameToken, Node startValueNode, Node endValueNode, Node stepValueNode, Node bodyNode, bool returnNull)
        {
            VarNameToken = varNameToken;
            StartValueNode = startValueNode;
            EndValueNode = endValueNode;
            StepValueNode = stepValueNode;
            BodyNode = bodyNode;
            ReturnNull = returnNull;

            Start = varNameToken.Start;
            End = bodyNode.End;
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();
            List<Object> elements = new List<Object>();

            Number startValue = (Number)res.Register(StartValueNode.Visit(context));
            if (res.ShouldReturn()) return res;

            Number endValue = (Number)res.Register(EndValueNode.Visit(context));
            if (res.ShouldReturn()) return res;

            Number stepValue;
            if (StepValueNode != null)
            {
                stepValue = (Number)res.Register(StepValueNode.Visit(context));
            }
            else
            {
                stepValue = new Number(1);
            }

            for (int i = (int)startValue.Value; stepValue.Value >= 0 ? i < endValue.Value : i > endValue.Value; i += (int)stepValue.Value)
            {
                context.SymbolTable.Set(VarNameToken.Value, new Number(i), false);

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
