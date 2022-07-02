using Stenguage.Errors;

namespace Stenguage.Objects
{
    public class Boolean : Object
    {
        public static Boolean False { get; } = new Boolean(false);
        public static Boolean True { get; } = new Boolean(true);

        public new bool Value;

        public Boolean(bool value)
        {
            Value = value;
        }

        public override (Boolean, Error) GetComparisonEE(Object other)
        {
            if (other is Boolean)
            {
                return ((Boolean)new Boolean(Value == ((Boolean)other).Value).SetContext(Context), null);
            }
            return (null, null);
        }

        public override (Boolean, Error) GetComparisonNE(Object other)
        {
            if (other is Boolean)
            {
                return ((Boolean)new Boolean(Value != ((Boolean)other).Value).SetContext(Context), null);
            }
            return (null, null);
        }

        public override (Boolean, Error) And(Object other)
        {
            if (other is Boolean)
            {
                return ((Boolean)new Boolean(Value && ((Boolean)other).Value).SetContext(Context), null);
            }
            return (null, new InvalidSyntaxError(Start, other.End, "Can only do 'and' operation with other booleans"));
        }

        public override (Boolean, Error) Or(Object other)
        {
            if (other is Boolean)
            {
                return ((Boolean)new Boolean(Value && ((Boolean)other).Value).SetContext(Context), null);
            }
            return (null, new InvalidSyntaxError(Start, other.End, "Can only do 'or' operation with other booleans"));
        }

        public override (Boolean, Error) Not()
        {
            return ((Boolean)new Boolean(!Value).SetContext(Context), null);
        }

        public override Object Copy()
        {
            Boolean copy = new Boolean(Value);
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
