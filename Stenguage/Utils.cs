using System;
using System.Text;

namespace Stenguage
{
    public static class Utils
    {
        public static string Repeat(this string s, int n) => new StringBuilder(s.Length * n).Insert(0, s, n).ToString();

        public static string StringWithArrows(string text, Position start, Position end)
        {
            return text + "\n" + new string(' ', Math.Max(start.Column, 0)) + new string('^', Math.Max(end.Column - start.Column, 0));
        }

    }
}