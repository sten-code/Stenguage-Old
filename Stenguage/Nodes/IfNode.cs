using Stenguage.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Stenguage.Nodes
{
    public class IfNode : Node
    {
        public List<(Node, Node, bool)> Cases;
        public (Node, bool) ElseCase;

        public IfNode(List<(Node, Node, bool)> cases, (Node, bool) elseCase)
        {
            Cases = cases;
            ElseCase = elseCase;

            Start = cases[0].Item1.Start;
            End = elseCase.Item1 != null ? elseCase.Item1.End : cases.Last().Item1.End;
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();

            foreach ((Node condition, Node expr, bool returnNull) in Cases)
            {
                Boolean conditionValue = (Boolean)res.Register(condition.Visit(context));
                if (res.ShouldReturn()) return res;

                if (conditionValue.Value)
                {
                    Object exprValue = res.Register(expr.Visit(context));
                    if (res.ShouldReturn()) return res;
                    return res.Success(returnNull ? Number.Null : exprValue);
                }
            }

            if (ElseCase.Item1 != null)
            {
                Object elseValue = res.Register(ElseCase.Item1.Visit(context));
                if (res.ShouldReturn()) return res;
                return res.Success(ElseCase.Item2 ? Number.Null : elseValue);
            }

            return res.Success(Number.Null);
        }
    }
}
