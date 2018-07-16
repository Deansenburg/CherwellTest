
namespace CherwellTest.Model
{
    public class Triangle
    {
        public Triangle(char r, int col, int[][] points)
        {
            Row = r;
            Column = col;
            Points = points;
        }
        public char Row { get; }
        public int Column { get; }
        public int[][] Points { get; }
    }
}