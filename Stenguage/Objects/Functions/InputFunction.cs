using System.Collections.Generic;

namespace Stenguage.Objects.Functions
{
    public class InputFunction : CSharpFunction
    {
        public InputFunction() : base("input", new Dictionary<string, ObjectType>() { })
        { }

        public override RuntimeResult Execute(Context context)
        {
            string input = System.Console.ReadLine();
            return new RuntimeResult().Success(new String(input));
        }

    }
}
