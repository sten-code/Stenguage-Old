using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class Mod : CSharpFunction
    {
        public Mod() : base("math.mod", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(Number)), ["mod"] = new ObjectType(typeof(Number)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            return new RuntimeResult().Success(new Number(context.SymbolTable.Get<Number>("value").Value % context.SymbolTable.Get<Number>("mod").Value));
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
