using Stenguage.Errors;

namespace Stenguage.Objects
{
    public class ObjectType : Object
    {
        public new System.Type Value;
        
        public ObjectType(System.Type value)
        {
            Value = value;
        }

        public override (Object, Error) AddedTo(Object other)
        {
            if (other is String)
            {
                return (new String(Value + ((String)other).Value).SetContext(Context), null);
            }
            return (null, IllegalOperation(other));
        }

        public override (Boolean, Error) GetComparisonEE(Object other)
        {
            if (other is ObjectType)
            {
                return ((Boolean)new Boolean(Value.Equals(((ObjectType)other).Value)).SetContext(Context), null);
            }
            return (null, IllegalOperation(other));
        }

        public override (Boolean, Error) GetComparisonNE(Object other)
        {
            if (other is ObjectType)
            {
                return ((Boolean)new Boolean(!Value.Equals(((ObjectType)other).Value)).SetContext(Context), null);
            }
            return (null, IllegalOperation(other));
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
