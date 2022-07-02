using Stenguage.Errors;
using System.Collections.Generic;
using System.Linq;

namespace Stenguage.Objects.Functions
{
    public abstract class BaseFunction : Object
    {
        public string Name;
        public Dictionary<string, ObjectType> ArgNames;

        public BaseFunction(string name, Dictionary<string, ObjectType> argNames) : base()
        {
            Name = name == string.Empty ? "<anonymous>" : name;
            ArgNames = argNames;
        }

        public abstract RuntimeResult Execute(List<Object> args);

        public Context GenerateNewContext()
        {
            Context newContext = new Context(Name, Context, Start);
            newContext.SymbolTable = new SymbolTable(newContext.Parent.SymbolTable);
            return newContext;
        }

        public RuntimeResult CheckArgs(Dictionary<string, ObjectType> argNames, List<Object> args)
        {
            RuntimeResult res = new RuntimeResult();

            if (args.Count > argNames.Count)
            {
                return res.Failure(new RuntimeError(Start, End, $"{args.Count - argNames.Count} too many args passed into '{Name}'", Context));
            }

            if (args.Count < argNames.Count)
            {
                return res.Failure(new RuntimeError(Start, End, $"{argNames.Count - args.Count} too few args passed into '{Name}'", Context));
            }

            return res.Success(null);
        }

        public void PopulateArgs(Dictionary<string, ObjectType> argNames, List<Object> args, Context context)
        {
            for (int i = 0; i < args.Count; i++)
            {
                string argName = argNames.ElementAt(i).Key;
                Object argValue = args[i];
                argValue.SetContext(context);
                context.SymbolTable.Set(argName, argValue, false);
            }
        }

        public RuntimeResult CheckAndPopulateArgs(Dictionary<string, ObjectType> argNames, List<Object> args, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            res.Register(CheckArgs(argNames, args));
            if (res.ShouldReturn()) return res;
            PopulateArgs(argNames, args, context);
            return res.Success(null);
        }

        public override string ToString()
        {
            return $"<function {Name}>";
        }
    }
}
