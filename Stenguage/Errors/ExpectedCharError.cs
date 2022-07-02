namespace Stenguage.Errors
{
    public class ExpectedCharError : Error
    {
        public ExpectedCharError(Position start, Position end, string details) : base(start, end, "Expected Character", details) { }
    }
}
