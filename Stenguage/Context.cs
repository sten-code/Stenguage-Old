namespace Stenguage
{
    public class Context
    {
        public string DisplayName;
        public Context Parent;
        public Position ParentEntryPosition;
        public SymbolTable SymbolTable;

        public Context(string displayName, Context parent = null, Position parentEntryPos = null)
        {
            DisplayName = displayName;
            Parent = parent;
            ParentEntryPosition = parentEntryPos;
            SymbolTable = null;
        }
    }
}
