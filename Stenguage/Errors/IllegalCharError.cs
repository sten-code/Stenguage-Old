namespace Stenguage.Errors
{
    public class IllegalCharError : Error
    {
        public IllegalCharError(Position start, Position end, string details) : base(start, end, "Illegal Character", details) { }
    }
}
