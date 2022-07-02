using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class LastIndexOf : CSharpFunction
    {
        public LastIndexOf() : base("string.lastindexof", new Dictionary<string, ObjectType>() { ["str"] = new ObjectType(typeof(String)), ["value"] = new ObjectType(typeof(String)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Number(context.SymbolTable.Get<String>("str").Value.LastIndexOf(context.SymbolTable.Get<String>("value").Value)));
        }

        public override Object Copy()
        {
            LastIndexOf copy = new LastIndexOf();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }
    }
}
