namespace Stenguage.Nodes
{
    public class BreakNode : Node
    {
        public BreakNode(Position start, Position end)
        {
            Start = start;
            End = end;
        }

        public override RuntimeResult Visit(Context context)
        {
            return new RuntimeResult().SuccessBreak();
        }
    }
}
