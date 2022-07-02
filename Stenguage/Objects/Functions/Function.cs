using Stenguage.Errors;
using Stenguage.Nodes;
using System.Collections.Generic;

namespace Stenguage.Objects.Functions
{
    public class Function : BaseFunction
    {
        public Node BodyNode;
        public bool AutoReturn;

        public Function(string name, Node bodyNode, Dictionary<string, ObjectType> argNames, bool autoReturn) : base(name, argNames)
        {
            BodyNode = bodyNode;
            AutoReturn = autoReturn;
        }

        public override RuntimeResult Execute(List<Object> args)
        {
            RuntimeResult res = new RuntimeResult();
            Context context = GenerateNewContext();

            res.Register(CheckAndPopulateArgs(ArgNames, args, context));
            if (res.ShouldReturn()) return res;

            Object value = res.Register(BodyNode.Visit(context));
            if (res.ShouldReturn() && res.FuncReturnValue == null) return res;

            return res.Success(AutoReturn ? value : res.FuncReturnValue != null ? res.FuncReturnValue : Number.Null);
        }

        public override Object Copy()
        {
            Function copy = new Function(Name, BodyNode, ArgNames, AutoReturn);
            copy.SetContext(Context);
            copy.SetPosition(Start, End);
            return copy;
        }
    }
}
