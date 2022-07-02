namespace Stenguage.Errors
{
    public class InvalidSyntaxError : Error
    {
        public InvalidSyntaxError(Position start, Position end, string details) : base(start, end, "Invalid Syntax", details) { }
    }
}
