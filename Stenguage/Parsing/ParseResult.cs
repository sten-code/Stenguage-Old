using Stenguage.Errors;
using Stenguage.Nodes;

namespace Stenguage.Parsing
{
    public class ParseResult
    {
        public Error Error;
        public object Value;
        private int AdvanceCount;
        private int LastRegisteredAdvanceCount;
        public int ToReverseCount;

        public ParseResult()
        {
            Error = null;
            Value = null;
            AdvanceCount = 0;
            LastRegisteredAdvanceCount = 0;
            ToReverseCount = 0;
        }

        public void RegisterAdvancement()
        {
            LastRegisteredAdvanceCount = 1;
            AdvanceCount++;
        }

        public object Register(ParseResult res)
        {
            LastRegisteredAdvanceCount = res.AdvanceCount;
            AdvanceCount += res.AdvanceCount;
            if (res.Error != null) Error = res.Error;
            return res.Value;
        }

        public object TryRegister(ParseResult res)
        {
            if (res.Error != null)
            {
                ToReverseCount = res.AdvanceCount;
                return null;
            }
            return Register(res);
        }

        public ParseResult Success(object value)
        {
            Value = value;
            return this;
        }

        public ParseResult Failure(Error error)
        {
            if (Error == null || LastRegisteredAdvanceCount == 0) Error = error;
            return this;
        }
    }
}
