using System.Collections.Generic;

namespace Stenguage.Objects.Functions
{
    public class StrFunction : CSharpFunction
    {
        public StrFunction() : base("str", new Dictionary<string, ObjectType> { ["value"] = new ObjectType(typeof(Object)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new String(context.SymbolTable.Get("value").ToString()));
        }

        public override Object Copy()
        {
            StrFunction copy = new StrFunction();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }
    }
}
