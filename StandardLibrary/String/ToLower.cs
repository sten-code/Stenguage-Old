using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class ToLower : CSharpFunction
    {
        public ToLower() : base("string.tolower", new Dictionary<string, ObjectType>() { ["str"] = new ObjectType(typeof(String)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new String(context.SymbolTable.Get<String>("str").Value.ToLower()));
        }

        public override Object Copy()
        {
            ToLower copy = new ToLower();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }
    }
}
