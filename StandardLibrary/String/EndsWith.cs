using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class EndsWith : CSharpFunction
    {
        public EndsWith() : base("string.endswith", new Dictionary<string, ObjectType>() { ["str"] = new ObjectType(typeof(String)), ["value"] = new ObjectType(typeof(String)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Boolean(context.SymbolTable.Get<String>("str").Value.EndsWith(context.SymbolTable.Get<String>("value").Value)));
        }

    }
}
