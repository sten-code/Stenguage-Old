using Stenguage.Errors;
using Stenguage.Nodes;
using Stenguage.Objects;
using Stenguage.Objects.Functions;
using Stenguage.Parsing;
using System.Collections.Generic;

namespace Stenguage
{
    public class SymbolTable
    {
        public Dictionary<string, (Object, bool)> Symbols;
        public SymbolTable Parent;

        public SymbolTable(SymbolTable parent=null) 
        {
            Symbols = new Dictionary<string, (Object, bool)>();
            Parent = parent;
        }

        public Object Get(string name)
        {
            Object value = Symbols.ContainsKey(name) ? Symbols[name].Item1 : null;
            if (value == null && Parent != null)
            {
                return Parent.Get(name);
            }
            return value;
        }

        public T Get<T>(string name) where T: Object
        {
            Object value = Symbols.ContainsKey(name) ? Symbols[name].Item1 : null;
            if (value == null && Parent != null)
            {
                return Parent.Get<T>(name);
            }
            return (T)value;
        }

        public void Set(string name, Object value, bool readOnly)
        {
            if (Symbols.ContainsKey(name) && Symbols[name].Item2)
            {
                return;
            }
            Symbols[name] = (value, readOnly);
        }

        public void Set(string name, Object value)
        {
            if (Symbols.ContainsKey(name) && Symbols[name].Item2)
            {
                return;
            }
            Symbols[name] = (value, Symbols[name].Item2);
        }

    }

    public class Stenguage
    {
        public SymbolTable GlobalSymbolTable;

        public Stenguage()
        {
            GlobalSymbolTable = new SymbolTable();

            // Variables
            GlobalSymbolTable.Set("null", Number.Null, true);
            GlobalSymbolTable.Set("true", Boolean.True, true);
            GlobalSymbolTable.Set("false", Boolean.False, true);
            GlobalSymbolTable.Set("pi", new Number((float)System.Math.PI), true);

            // Types
            GlobalSymbolTable.Set(typeof(String).Name, new ObjectType(typeof(String)), true);
            GlobalSymbolTable.Set(typeof(Number).Name, new ObjectType(typeof(Number)), true);
            GlobalSymbolTable.Set(typeof(Boolean).Name, new ObjectType(typeof(Boolean)), true);
            GlobalSymbolTable.Set(typeof(List).Name, new ObjectType(typeof(List)), true);
            GlobalSymbolTable.Set(typeof(ObjectType).Name, new ObjectType(typeof(ObjectType)), true);
            GlobalSymbolTable.Set(typeof(Function).Name, new ObjectType(typeof(Function)), true);
            GlobalSymbolTable.Set(typeof(CSharpFunction).Name, new ObjectType(typeof(CSharpFunction)), true);

            // Functions
            GlobalSymbolTable.Set("print", new PrintFunction(), true);
            GlobalSymbolTable.Set("input", new InputFunction(), true);
            GlobalSymbolTable.Set("instanceof", new InstanceofFunction(), true);
            GlobalSymbolTable.Set("len", new LenFunction(), true);
            GlobalSymbolTable.Set("import", new ImportFunction(), true);
            GlobalSymbolTable.Set("str", new StrFunction(), true);
        }

        public (Object, Error) Run(string fileName, string script)
        {
            Lexer lexer = new Lexer(fileName, script);
            (List<Token> tokens, Error error) = lexer.MakeTokens();
            if (error != null) return (null, error);

            Parser parser = new Parser(tokens);
            ParseResult res = parser.Parse();
            //foreach(Token token in tokens) System.Console.WriteLine(token);
            if (res.Error != null) return (null, res.Error);

            Context context = new Context("<program>");
            context.SymbolTable = GlobalSymbolTable;
            RuntimeResult result = ((Node)res.Value).Visit(context);
            return (result.Value, result.Error);
        }
    }
}
