using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class Sqrt : CSharpFunction
    {
        public Sqrt() : base("math.sqrt", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(Number)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Number((float)System.Math.Sqrt(context.SymbolTable.Get<Number>("value").Value)));
        }

        public override Object Copy()
        {
            Sqrt copy = new Sqrt();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }
    }
}
