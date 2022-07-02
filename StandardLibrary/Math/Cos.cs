using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class Cos : CSharpFunction
    {
        public Cos() : base("math.cos", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(Number)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Number((float)System.Math.Cos(context.SymbolTable.Get<Number>("value").Value)));
        }

        public override Object Copy()
        {
            Cos copy = new Cos();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }

    }
}
