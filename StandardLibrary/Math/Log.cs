using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class Log : CSharpFunction
    {
        public Log() : base("math.log", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(Number)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Number((float)System.Math.Log(context.SymbolTable.Get<Number>("value").Value)));
        }

        public override Object Copy()
        {
            Log copy = new Log();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }

    }
}
