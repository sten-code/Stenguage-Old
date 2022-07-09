using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class Contains : CSharpFunction
    {
        public Contains() : base("string.contains", new Dictionary<string, ObjectType>() { ["str"] = new ObjectType(typeof(String)), ["value"] = new ObjectType(typeof(String)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Boolean(context.SymbolTable.Get<String>("str").Value.Contains(context.SymbolTable.Get<String>("value").Value)));
        }

    }
}
