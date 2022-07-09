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
            return (null, IllegalOperation(other));

        }

        public override (Object, Error) MultipliedBy(Object other)
        {
            if (other is Number)
            {
                int amount = int.Parse(((Number)other).Value.ToString());
                return (new String(Value.Repeat(amount)).SetContext(Context), null);
            }
            return (null, IllegalOperation(other));

        }

        public override (Object, Error) GetIndex(Object other)
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
        
        public override (Object, Error) SetIndex(Object index, Object value)
        {
            if (index is Number)
            {
                if (int.TryParse(((Number)index).Value.ToString(), out int i))
                {
                    return (new String(Value.Remove(i, 1).Insert(i, value.ToString())), null);
                }
                else
                {
                    return (null, new RuntimeError(Start, index.End, "The index given wasn't a valid number.", Context));
                }
            }
            return (null, IllegalOperation(index));
        }

        public override string ToString()
        {
            return Value;
        }

    }
}
