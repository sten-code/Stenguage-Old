using Stenguage.Errors;
using System.Collections.Generic;

namespace Stenguage.Objects.Functions
{
    public class LenFunction : CSharpFunction
    {
        public LenFunction() : base("len", new Dictionary<string, ObjectType> { ["value"] = new ObjectType(typeof(Object)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            RuntimeResult res = new RuntimeResult();
            Object value = context.SymbolTable.Get("value");
            if (value is String)
            {
                int length = ((String)value).Value.Length;
                return res.Success(new Number(length));
            }
            else if (value is List)
            {
                int length = ((List)value).Elements.Count;
                return res.Success(new Number(length));
            }
            else if (value is Number)
            {
                int length = ((Number)value).Value.ToString().Length;
                return res.Success(new Number(length));
            }
            return res.Failure(new RuntimeError(Start, End, $"The object given isn't a valid type, valid types: 'String', 'Number' and 'List'", context));
        }

        public override Object Copy()
        {
            LenFunction copy = new LenFunction();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }
    }
}
