using Stenguage;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using System.Collections.Generic;
using System.Linq;

namespace StandardLibrary
{
    public class Split : CSharpFunction
    {
        public Split() : base("string.split", new Dictionary<string, ObjectType>() { ["str"] = new ObjectType(typeof(String)), ["seperator"] = new ObjectType(typeof(String)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            string str = context.SymbolTable.Get<String>("str").Value;
            string seperator = context.SymbolTable.Get<String>("seperator").Value;
            List list = new List(str.Split(new string[] { seperator }, System.StringSplitOptions.None).Select(x => (Object)new String(x)).ToList());
            return new RuntimeResult().Success(list);
        }

    }
}
