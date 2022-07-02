using Stenguage.Errors;
using Stenguage.Objects;

namespace Stenguage.Nodes
{
    public class BinaryOperationNode : Node
    {
        public Node LeftNode;
        public Token OperatorToken;
        public Node RightNode;

        public BinaryOperationNode(Node leftNode, Token operatorToken, Node rightNode)
        {
            LeftNode = leftNode;
            OperatorToken = operatorToken;
            RightNode = rightNode;
            Start = leftNode.Start;
            End = rightNode.End;
        }

        public override string ToString()
        {
            return $"({LeftNode}, {OperatorToken}, {RightNode})";
        }

        public override RuntimeResult Visit(Context context)
        {
            RuntimeResult res = new RuntimeResult();
            Object left = res.Register(LeftNode.Visit(context));
            if (res.ShouldReturn()) return res;
            Object right = res.Register(RightNode.Visit(context));
            if (res.ShouldReturn()) return res;

            Object num = null;
            Error error = null;
            if (OperatorToken.Type == Token.TT_PLUS)
            {
                (num, error) = left.AddedTo(right);
            }
            else if (OperatorToken.Type == Token.TT_MINUS)
            {
                (num, error) = left.SubtractedFrom(right);
            }
            else if (OperatorToken.Type == Token.TT_MUL)
            {
                (num, error) = left.MultipliedBy(right);
            }
            else if (OperatorToken.Type == Token.TT_DIV)
            {
                (num, error) = left.DividedBy(right);
            }
            else if (OperatorToken.Type == Token.TT_POW)
            {
                (num, error) = left.Power(right);
            }
            else if (OperatorToken.Type == Token.TT_EE)
            {
                (num, error) = left.GetComparisonEE(right);
            }
            else if (OperatorToken.Type == Token.TT_NE)
            {
                (num, error) = left.GetComparisonNE(right);
            }
            else if (OperatorToken.Type == Token.TT_LT)
            {
                (num, error) = left.GetComparisonLT(right);
            }
            else if (OperatorToken.Type == Token.TT_GT)
            {
                (num, error) = left.GetComparisonGT(right);
            }
            else if (OperatorToken.Type == Token.TT_LTE)
            {
                (num, error) = left.GetComparisonLTE(right);
            }
            else if (OperatorToken.Type == Token.TT_GTE)
            {
                (num, error) = left.GetComparisonGTE(right);
            }
            else if (OperatorToken.Matches(Token.TT_KEYWORD, "and"))
            {
                (num, error) = left.And(right);
            }
            else if (OperatorToken.Matches(Token.TT_KEYWORD, "or"))
            {
                (num, error) = left.Or(right);
            }

            if (error != null) return res.Failure(error);
            else return res.Success(num.SetPosition(Start, End));
        }
    }

}
