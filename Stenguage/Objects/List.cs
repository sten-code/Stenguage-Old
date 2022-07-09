using Stenguage.Errors;
using System;
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
            List list = (List)MemberwiseClone();
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
                List list = (List)MemberwiseClone();
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

        public override (Object, Error) GetIndex(Object other)
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

        public override (Object, Error) SetIndex(Object index, Object value)
        {
            if (index is Number)
            {
                try
                {
                    Object[] elements = new Object[Elements.Count];
                    Elements.CopyTo(elements);
                    int.TryParse(((Number)index).Value.ToString(), out int i);
                    elements[i] = value;

                    return (new List(elements.ToList()), null);
                } catch
                {
                    return (null, new RuntimeError(Start, End, "Index out of bounds.", Context));
                }
            }
            return (null, IllegalOperation(index));
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", Elements.Select(x => x.ToString()))}]";
        }

    }
}
