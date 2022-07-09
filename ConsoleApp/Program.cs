using Stenguage.Errors;
using Stenguage.Objects;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                System.Console.Write("> ");
                string input = System.Console.ReadLine();
                if (input != "")
                {
                    Stenguage.Stenguage stenguage = new Stenguage.Stenguage();
                    (Object output, Error error) = stenguage.Run("<program>", input);
                    if (error != null) System.Console.WriteLine(error);
                }
            }
        }
    }
}
