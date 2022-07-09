using Stenguage;
using Stenguage.Errors;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class Round : CSharpFunction
    {
        public Round() : base("math.round", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(Number)), ["decimals"] = new ObjectType(typeof(Number)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            RuntimeResult res = new RuntimeResult();
            if (int.TryParse(context.SymbolTable.Get<Number>("decimals").Value.ToString(), out int decimals))
            {
                return res.Success(new Number((float)System.Math.Round(context.SymbolTable.Get<Number>("value").Value, decimals)));
            } else
            {
                return res.Failure(new RuntimeError(Start, End, "The input 'decimals' isn't an integer, you must input a full number", context));
            }
        }

    }
}
