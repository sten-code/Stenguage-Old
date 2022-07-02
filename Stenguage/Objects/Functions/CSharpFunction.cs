using System.Collections.Generic;

namespace Stenguage.Objects.Functions
{
    public abstract class CSharpFunction : BaseFunction
    {
        public CSharpFunction(string name, Dictionary<string, ObjectType> argNames) : base(name, argNames)
        { }

        public abstract RuntimeResult Execute(Context context);

        public override RuntimeResult Execute(List<Object> args)
        {
            RuntimeResult res = new RuntimeResult();
            Context context = GenerateNewContext();
            context.Parent = Context;
            res.Register(CheckAndPopulateArgs(ArgNames, args, context));
            if (res.ShouldReturn()) return res;

            Object value = res.Register(Execute(context));
            if (res.ShouldReturn()) return res;

            return res.Success(value);
        }
    }
}
