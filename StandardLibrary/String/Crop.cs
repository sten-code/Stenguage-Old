using Stenguage;
using Stenguage.Errors;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;

namespace StandardLibrary
{
    public class Crop : CSharpFunction
    {
        public Crop() : base("string.crop", new Dictionary<string, ObjectType>() { ["str"] = new ObjectType(typeof(String)), ["startIndex"] = new ObjectType(typeof(Number)), ["endIndex"] = new ObjectType(typeof(Number)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            RuntimeResult res = new RuntimeResult();
            if (int.TryParse(context.SymbolTable.Get<Number>("startIndex").Value.ToString(), out int startIndex))
            {
                if (int.TryParse(context.SymbolTable.Get<Number>("endIndex").Value.ToString(), out int endIndex))
                {
                    return res.Success(new String(context.SymbolTable.Get<String>("str").Value.Substring(startIndex, endIndex - startIndex)));
                }
                else
                {
                    return res.Failure(new RuntimeError(Start, End, "The input 'endIndex' has to be an integer (full number)", context));
                }
            } else
            {
                return res.Failure(new RuntimeError(Start, End, "The input 'startIndex' has to be an integer (full number)", context));
            }
        }

        public override Object Copy()
        {
            Crop copy = new Crop();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }
    }
}
