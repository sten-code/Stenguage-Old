using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class Ceiling : CSharpFunction
    {
        public Ceiling() : base("math.ceiling", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(Number)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Number((float)System.Math.Ceiling(context.SymbolTable.Get<Number>("value").Value)));
        }

        public override Object Copy()
        {
            Ceiling copy = new Ceiling();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }

    }
}
