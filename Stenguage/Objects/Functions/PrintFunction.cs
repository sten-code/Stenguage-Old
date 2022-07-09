using System;
using System.Collections.Generic;

namespace Stenguage.Objects.Functions
{
    public class PrintFunction : CSharpFunction
    {
        public PrintFunction() : base("print", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(Object)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            Console.WriteLine(context.SymbolTable.Get("value"));
            return new RuntimeResult().Success(Number.Null);
        }

    }
}
