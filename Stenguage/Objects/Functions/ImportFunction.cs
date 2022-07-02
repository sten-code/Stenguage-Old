using Stenguage.Errors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Stenguage.Objects.Functions
{
    public class ImportFunction : CSharpFunction
    {
        public ImportFunction() : base("import", new Dictionary<string, ObjectType>() { ["file"] = new ObjectType(typeof(String)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            RuntimeResult res = new RuntimeResult();
            string libName = context.SymbolTable.Get<String>("file").Value;
            string extension = Path.GetExtension(libName);
            if (extension == ".sten")
            {
                if (!File.Exists(libName + ".sten")) return res.Failure(new RuntimeError(Start, End, $"The library '{libName}' doesn't exist.", context));
                ImportScript(context, libName);
                return res.Success(Number.Null);
            }
            else if (extension == ".dll")
            {
                if (!File.Exists(libName + ".dll")) return res.Failure(new RuntimeError(Start, End, $"The library '{libName}' doesn't exist.", context));
                ImportDll(context, libName);
                return res.Success(Number.Null);
            }
            else if (extension == "")
            {
                bool imported = false;
                if (File.Exists(libName + ".dll"))
                {
                    imported = true;
                    ImportDll(context, libName + ".dll");
                }
                if (File.Exists(libName + ".sten"))
                {
                    imported = true;
                    ImportScript(context, libName + ".sten");
                }
                if (!imported)
                {
                    return res.Failure(new RuntimeError(Start, End, $"The library '{libName}' does not exist", context));
                }
                return res.Success(Number.Null);
            } 
            else
            {
                return res.Failure(new RuntimeError(Start, End, "", context));
            }
        }

        public void ImportDll(Context context, string libName)
        {
            Assembly asm = Assembly.LoadFrom(libName);
            foreach (Type type in asm.GetTypes())
            {
                if (type.BaseType == typeof(CSharpFunction))
                {
                    CSharpFunction function = Activator.CreateInstance(type) as CSharpFunction;
                    context.Parent.SymbolTable.Set(function.Name, function, true);
                }
                else if (type.BaseType == typeof(Object))
                {
                    Object obj = Activator.CreateInstance(type) as Object;
                    context.Parent.SymbolTable.Set(obj.GetType().Name, new ObjectType(obj.GetType()), true);
                }
            }
        }

        public void ImportScript(Context context, string libName)
        {
            string script = File.ReadAllText(libName);
            Stenguage lang = new Stenguage();
            (Object _, Error err) = lang.Run(libName, script);
            if (err != null) Console.WriteLine(err);

            foreach (KeyValuePair<string, (Object, bool)> pair in lang.GlobalSymbolTable.Symbols)
            {
                if (pair.Value.Item2) continue;
                context.Parent.SymbolTable.Set(pair.Key, pair.Value.Item1, pair.Value.Item2);
            }
        }

        public override Object Copy()
        {
            ImportFunction copy = new ImportFunction();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }
    }
}
