namespace Stenguage.Nodes
{
    public class ContinueNode : Node
    {
        public ContinueNode(Position start, Position end)
        {
            Start = start;
            End = end;
        }

        public override RuntimeResult Visit(Context context)
        {
            return new RuntimeResult().SuccessContinue();
        }
    }
}
