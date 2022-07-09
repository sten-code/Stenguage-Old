using Stenguage.Errors;

namespace Stenguage.Objects
{
    public abstract class Object
    {
        public Position Start;
        public Position End;
        public Context Context;
        public object Value;

        public Object()
        {
            SetPosition();
            SetContext();
        }

        public Object SetPosition(Position start = null, Position end = null)
        {
            Start = start;
            End = end;
            return this;
        }

        public Object SetContext(Context context = null)
        {
            Context = context;
            return this;
        }

        public virtual (Object, Error) GetIndex(Object other)
        {
            return (null, IllegalOperation(other));
        }

        public virtual (Object, Error) SetIndex(Object index, Object value)
        {
            return (null, IllegalOperation(index));
        }

        public virtual (Object, Error) AddedTo(Object other)
        {
            return (null, IllegalOperation(other));
        }

        public virtual (Object, Error) SubtractedFrom(Object other)
        {
            return (null, IllegalOperation(other));
        }

        public virtual (Object, Error) MultipliedBy(Object other)
        {
            return (null, IllegalOperation(other));
        }

        public virtual (Object, Error) DividedBy(Object other)
        {
            return (null, IllegalOperation(other));
        }

        public virtual (Object, Error) Power(Object other)
        {
            return (null, IllegalOperation(other));
        }

        public virtual (Boolean, Error) GetComparisonEE(Object other)
        {
            return (new Boolean(Value.Equals(other.Value)), null);
        }

        public virtual (Boolean, Error) GetComparisonNE(Object other)
        {
            return (new Boolean(!Value.Equals(other.Value)), null);
        }

        public virtual (Boolean, Error) GetComparisonLT(Object other)
        {
            return (null, IllegalOperation(other));
        }

        public virtual (Boolean, Error) GetComparisonGT(Object other)
        {
            return (null, IllegalOperation(other));
        }

        public virtual (Boolean, Error) GetComparisonLTE(Object other)
        {
            return (null, IllegalOperation(other));
        }

        public virtual (Boolean, Error) GetComparisonGTE(Object other)
        {
            return (null, IllegalOperation(other));
        }

        public virtual (Boolean, Error) And(Object other)
        {
            return (null, IllegalOperation(other));
        }

        public virtual (Boolean, Error) Or(Object other)
        {
            return (null, IllegalOperation(other));
        }

        public virtual (Boolean, Error) Not()
        {
            return (null, IllegalOperation());
        }

        public abstract Object Copy();

        public Error IllegalOperation(Object other = null)
        {
            if (other == null) other = this;
            return new RuntimeError(Start, other.End, "Illegal operation", Context);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
