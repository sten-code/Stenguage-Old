using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class ToUpper : CSharpFunction
    {
        public ToUpper() : base("string.toupper", new Dictionary<string, ObjectType>() { ["str"] = new ObjectType(typeof(String)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new String(context.SymbolTable.Get<String>("str").Value.ToUpper()));
        }

        public override Object Copy()
        {
            ToUpper copy = new ToUpper();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }
    }
}
