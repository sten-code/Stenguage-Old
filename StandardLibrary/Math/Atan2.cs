using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class Atan2 : CSharpFunction
    {
        public Atan2() : base("math.atan2", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(Number)), ["value2"] = new ObjectType(typeof(Number)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Number((float)System.Math.Atan2(context.SymbolTable.Get<Number>("value").Value, context.SymbolTable.Get<Number>("value2").Value)));
        }

    }
}
