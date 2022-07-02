using Stenguage.Errors;
using Stenguage.Objects.Functions;

namespace Stenguage.Objects
{
    public class ObjectType : Object
    {
        public new System.Type Value;
        
        public ObjectType(System.Type value)
        {
            Value = value;
        }

        public override (Boolean, Error) GetComparisonEE(Object other)
        {
            if (other is ObjectType)
            {
                return ((Boolean)new Boolean(Value.Equals(((ObjectType)other).Value)).SetContext(Context), null);
            }
            return (null, null);
        }

        public override (Boolean, Error) GetComparisonNE(Object other)
        {
            if (other is ObjectType)
            {
                return ((Boolean)new Boolean(!Value.Equals(((ObjectType)other).Value)).SetContext(Context), null);
            }
            return (null, null);
        }

        public override Object Copy()
        {
            ObjectType copy = new ObjectType(Value);
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }

        public override string ToString()
        {
            return Value.Name;
        }
    }
}
