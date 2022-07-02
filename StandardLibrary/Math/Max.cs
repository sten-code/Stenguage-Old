using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class Max : CSharpFunction
    {
        public Max() : base("math.max", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(Number)), ["value2"] = new ObjectType(typeof(Number)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Number((float)System.Math.Max(context.SymbolTable.Get<Number>("value").Value, context.SymbolTable.Get<Number>("value2").Value)));
        }

        public override Object Copy()
        {
            Max copy = new Max();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }

    }
}
