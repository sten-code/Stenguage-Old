using Stenguage.Errors;
using Stenguage.Objects;

namespace Stenguage
{
    public class RuntimeResult
    {
        public Object Value;
        public Error Error;
        public Object FuncReturnValue;
        public bool LoopContinue;
        public bool LoopBreak;

        public RuntimeResult()
        {
            Reset();
        }

        public void Reset()
        {
            Value = null;
            Error = null;
            FuncReturnValue = null;
            LoopContinue = false;
            LoopBreak = false;
        }

        public Object Register(RuntimeResult res)
        {
            if (res.ShouldReturn()) Error = res.Error;
            FuncReturnValue = res.FuncReturnValue;
            LoopContinue = res.LoopContinue;
            LoopBreak = res.LoopBreak;
            return res.Value;
        }

        public RuntimeResult Success(Object value)
        {
            Reset();
            Value = value;
            return this;
        }

        public RuntimeResult SuccessReturn(Object value)
        {
            Reset();
            FuncReturnValue = value;
            return this;
        }

        public RuntimeResult SuccessContinue()
        {
            Reset();
            LoopContinue = true;
            return this;
        }

        public RuntimeResult SuccessBreak()
        {
            Reset();
            LoopBreak = true;
            return this;
        }

        public RuntimeResult Failure(Error error)
        {
            Reset();
            Error = error;
            return this;
        }

        public bool ShouldReturn()
        {
            return Error != null || FuncReturnValue != null || LoopContinue || LoopBreak;
        }
    }
}
