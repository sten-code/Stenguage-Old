namespace Stenguage
{
    public class Position
    {
        public int Index;
        public int Line;
        public int Column;

        public string FileName;
        public string FileText;

        public Position(int index, int line, int column, string fileName, string fileText)
        {
            Index = index;
            Line = line;
            Column = column;

            FileName = fileName;
            FileText = fileText;
        }

        public Position Advance(char currentChar = char.MaxValue)
        {
            Index++;
            Column++;

            if (currentChar - '0' == -35 || currentChar.Equals('\n'))
            {
                Line++;
                Column = 0;
            }

            return this;
        }

        public Position Copy()
        {
            return new Position(Index, Line, Column, FileName, FileText);
        }
    }
}
