using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class Replace : CSharpFunction
    {
        public Replace() : base("string.replace", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(String)), ["old"] = new ObjectType(typeof(String)), ["new"] = new ObjectType(typeof(String)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new String(context.SymbolTable.Get<String>("value").Value.Replace(context.SymbolTable.Get<String>("old").Value, context.SymbolTable.Get<String>("new").Value)));
        }

    }
}
