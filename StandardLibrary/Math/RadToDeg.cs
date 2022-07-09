using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class RadToDeg : CSharpFunction
    {
        public RadToDeg() : base("math.radtodeg", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(Number)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Number((float)(context.SymbolTable.Get<Number>("value").Value*180/System.Math.PI)));
        }

    }
}
