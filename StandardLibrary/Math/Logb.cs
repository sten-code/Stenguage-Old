using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class Logb : CSharpFunction
    {
        public Logb() : base("math.logb", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(Number)), ["base"] = new ObjectType(typeof(Number)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Number((float)System.Math.Log(context.SymbolTable.Get<Number>("value").Value, context.SymbolTable.Get<Number>("base").Value)));
        }

    }
}
