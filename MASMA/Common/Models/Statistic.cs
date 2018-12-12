namespace MASMA.Common.Models
{
    public class Statistic
    {
        public string AgentName { get; set; }
        public int NoMessages { get; set; }
        public int NoAtomicOperation { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1} NoMessages; {2} NoAtomicOperation;",
                AgentName, NoMessages, NoAtomicOperation);
        }
    }
}
