using Stenguage.Errors;
using System.Collections.Generic;
using System.Linq;

namespace Stenguage.Objects
{
    public class List : Object
    {
        public List<Object> Elements;

        public List(List<Object> elements) : base()
        {
            Elements = elements;
        }

        public override (Object, Error) AddedTo(Object other)
        {
            List list = (List)Copy();
            if (other is List)
            {
                list.Elements.AddRange(((List)other).Elements);
            }
            else
            {
                list.Elements.Add(other);
            }
            return (list, null);
        }

        public override (Object, Error) SubtractedFrom(Object other)
        {
            if (other is Number)
            {
                List list = (List)Copy();
                try
                {
                    int index = int.Parse(((Number)other).Value.ToString());
                    list.Elements.RemoveAt(index);
                    return (list, null);
                }
                catch
                {
                    return (null, new RuntimeError(other.Start, other.End, $"Element at index '{((Number)other).Value}' could not be removed because the index was out of range.", Context));
                }
            }
            return (null, IllegalOperation(other));
        }

        public override (Object, Error) Index(Object other)
        {
            if (other is Number)
            {
                try
                {
                    int index = int.Parse(((Number)other).Value.ToString());
                    return (Elements[index], null);
                }
                catch
                {
                    return (null, new RuntimeError(other.Start, other.End, $"Element at index '{((Number)other).Value}' does not exist.", Context));
                }
            }
            return (null, IllegalOperation(other));
        }

        public override Object Copy()
        {
            List copy = new List(Elements);
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", Elements.Select(x => x.ToString()))}]";
        }
    }
}
