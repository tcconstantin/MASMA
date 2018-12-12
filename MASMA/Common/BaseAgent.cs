using ActressMas;
using MASMA.Common.Models;

namespace MASMA.Common
{
    public class BaseAgent : Agent
    {
        protected Statistic Statistic;

        public BaseAgent()
        {
            Statistic = new Statistic
            {
                AgentName = Name
            };
        }

        public BaseAgent(string name)
        {
            Name = name;

            Statistic = new Statistic
            {
                AgentName =  name
            };
        }

        public override void AfterSend()
        {
            Statistic.NoMessages++;
        }
    }
}
