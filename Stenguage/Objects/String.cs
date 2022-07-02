using Stenguage.Errors;

namespace Stenguage.Objects
{
    public class String : Object
    {
        public new string Value;

        public String(string value) : base()
        {
            Value = value;
        }

        public override (Object, Error) AddedTo(Object other)
        {
            if (other is String)
            {
                return (new String(Value + ((String)other).Value).SetContext(Context), null);
            }
            else if (other is Number)
            {
                return (new String(Value + ((Number)other).Value).SetContext(Context), null);
            }
            else
            {
                return (null, IllegalOperation(other));
            }
        }

        public override (Object, Error) MultipliedBy(Object other)
        {
            if (other is Number)
            {
                int amount = int.Parse(((Number)other).Value.ToString());
                return (new String(Value.Repeat(amount)).SetContext(Context), null);
            }
            else
            {
                return (null, IllegalOperation(other));
            }
        }

        public override (Object, Error) Index(Object other)
        {
            if (other is Number)
            {
                try
                {
                    int index = int.Parse(((Number)other).Value.ToString());
                    return (new String(Value[index].ToString()), null);
                }
                catch
                {
                    return (null, new RuntimeError(Start, other.End, $"The index '{((Number)other).Value}' is out of bounds.", Context));
                }
            }
            return (null, IllegalOperation(other));
        }

        public override Object Copy()
        {
            String copy = new String(Value);
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }

        public override string ToString()
        {
            return Value;
        }

    }
}
