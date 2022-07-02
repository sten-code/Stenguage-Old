using System;

namespace Stenguage.Errors
{
    public class RuntimeError : Error
    {
        public Context Context;

        public RuntimeError(Position start, Position end, string details, Context context) : base(start, end, "Runtime Error", details)
        {
            Context = context;
        }

        public override string ToString()
        {
            return $"{GenerateTraceback()}{Name}: {Details}\n\n{Utils.StringWithArrows(Start.FileText, Start, End)}";
        }

        public string GenerateTraceback()
        {
            string result = "";
            Position pos = Start;
            Context ctx = Context;
            while (ctx != null)
            {
                result = $" File {pos.FileName}, line {pos.Line + 1}, {ctx.DisplayName}\n{result}";
                pos = ctx.ParentEntryPosition;
                ctx = ctx.Parent;
            }

            return $"Traceback (most recent call last):\n{result}";
        }

    }
}
