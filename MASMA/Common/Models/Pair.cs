namespace MASMA.Common.Models
{
    public class Pair
    {
        public Pair(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int Start { get; set; }
        public int End { get; set; }
    }
}
