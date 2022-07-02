namespace Stenguage.Errors
{
    public class Error
    {
        public string Name { get; private set; }
        public string Details { get; private set; }

        public Position Start { get; private set; }
        public Position End { get; private set; }

        public Error(Position start, Position end, string name, string details)
        {
            Start = start;
            End = end;
            Name = name;
            Details = details;
        }

        public override string ToString()
        {
            return $"{Name}: {Details}\n File {Start.FileName}, line {Start.Line + 1}\n\n{Utils.StringWithArrows(Start.FileText, Start, End)}";
        }
    }
}
