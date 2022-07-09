using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class StartsWith : CSharpFunction
    {
        public StartsWith() : base("string.startswith", new Dictionary<string, ObjectType>() { ["str"] = new ObjectType(typeof(String)), ["value"] = new ObjectType(typeof(String)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Boolean(context.SymbolTable.Get<String>("str").Value.StartsWith(context.SymbolTable.Get<String>("value").Value)));
        }

    }
}
