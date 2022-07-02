using Stenguage.Errors;
using System;

namespace Console_Program
{
    public class Program
    {
        public static bool Running;
        public static Stenguage.Stenguage Stenguage;

        public static void Main(string[] args)
        {
            Stenguage = new Stenguage.Stenguage();
            Running = true;
            while (Running)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input.Equals(string.Empty)) continue;
                (Stenguage.Objects.Object result, Error error) = Stenguage.Run("<stdin>", input);

                if (error != null) Console.WriteLine(error);
            }
        }
    }
}
