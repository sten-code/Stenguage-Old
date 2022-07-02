using Stenguage.Errors;
using Stenguage.Objects;

namespace Stenguage.Nodes
{
    public class ListItemAssignNode : Node
    {
        public Token ListNameToken;
        public Node IndexNode;
        public Node ValueNode;

        public ListItemAssignNode(Token listName, Node indexNode, Node valueNode)
        {
            ListNameToken = listName;
            IndexNode = indexNode;
            ValueNode = valueNode;

            Start = listName.Start;
            End = valueNode.End;
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();

            string listName = ListNameToken.Value;
            Object value = res.Register(ValueNode.Visit(context));
            if (res.ShouldReturn()) return res;

            Number indexNumber = (Number)res.Register(IndexNode.Visit(context));
            int index = int.Parse(indexNumber.Value.ToString());
            List list = (List)context.SymbolTable.Get<List>(listName).Copy();

            if (index < 0 || index > list.Elements.Count - 1)
                return res.Failure(new RuntimeError(Start, End, $"Index out of bounds. index: {index}, last index: {list.Elements.Count - 1}", context));

            list.Elements[index] = value;
            context.SymbolTable.Set(listName, list, false);

            return res.Success(list);
        }
    }
}
