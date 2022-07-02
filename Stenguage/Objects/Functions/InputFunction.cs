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

        public override Object Copy()
        {
            InputFunction copy = new InputFunction();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }
    }
}
