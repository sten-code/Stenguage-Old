namespace Stenguage
{
    public class Token
    {
        public static string TT_INT = "TT_INT";
        public static string TT_FLOAT = "FLOAT";
        public static string TT_STRING = "STRING";
        public static string TT_IDENTIFIER = "IDENTIFIER";
        public static string TT_KEYWORD = "KEYWORD";
        public static string TT_PLUS = "PLUS";
        public static string TT_MINUS = "MIN";
        public static string TT_MUL = "MUL";
        public static string TT_DIV = "DIV";
        public static string TT_POW = "POW";
        public static string TT_EQ = "EQ";
        public static string TT_LPAREN = "LPAREN";
        public static string TT_RPAREN = "RPAREN";
        public static string TT_LBRACK = "LBRACK";
        public static string TT_RBRACK = "RBRACK";
        public static string TT_EE = "EE";
        public static string TT_NE = "NE";
        public static string TT_LT = "LT";
        public static string TT_GT = "GT";
        public static string TT_LTE = "LTE";
        public static string TT_GTE = "GTE";
        public static string TT_COMMA = "COMMA";
        public static string TT_ARROW = "ARROW";
        public static string TT_NEWLINE = "NEWLINE";
        public static string TT_EOF = "EOF";
        public static string[] KEYWORDS = new string[] 
        { 
            "var", 
            "and", "or", "not", 
            "if", "then", "elif", "else", 
            "for", "to", "step", "while", "continue", "break",
            "func", "end", "return" 
        };

        public string Type;
        public string Value;
        public Position Start;
        public Position End;

        public Token(string type, string value = null, Position start = null, Position end = null)
        {
            Type = type;
            Value = value;

            if (start != null)
            {
                Start = start.Copy();
                End = start.Copy();
                End.Advance();
            }
            if (end != null) End = end.Copy();
        }

        public bool Matches(string type, string value)
        {
            return Type == type && Value == value;
        }

        public override string ToString()
        {
            if (Value != null) return $"{Type}:{Value}";
            return $"{Type}";
        }
    }
}
