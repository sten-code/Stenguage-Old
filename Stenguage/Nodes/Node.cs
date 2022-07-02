namespace Stenguage.Nodes
{
    public abstract class Node
    {
        public Position Start;
        public Position End;

        public Node() { }

        public abstract RuntimeResult Visit(Context context);

        public override string ToString()
        {
            return "<Empty node>";
        }
    }
}
