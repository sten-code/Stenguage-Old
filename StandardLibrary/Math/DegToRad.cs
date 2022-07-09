using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class DegToRad : CSharpFunction
    {
        public DegToRad() : base("math.degtorad", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(Number)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Number((float)(context.SymbolTable.Get<Number>("value").Value*System.Math.PI/180)));
        }

    }
}
