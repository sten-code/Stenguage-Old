using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class Sin : CSharpFunction
    {
        public Sin() : base("math.sin", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(Number)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Number((float)System.Math.Sin(context.SymbolTable.Get<Number>("value").Value)));
        }

        public override Object Copy()
        {
            Sin copy = new Sin();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }

    }
}
