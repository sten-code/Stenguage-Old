using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class IndexOf : CSharpFunction
    {
        public IndexOf() : base("string.indexof", new Dictionary<string, ObjectType>() { ["str"] = new ObjectType(typeof(String)), ["value"] = new ObjectType(typeof(String)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Number(context.SymbolTable.Get<String>("str").Value.IndexOf(context.SymbolTable.Get<String>("value").Value)));
        }

    }
}
