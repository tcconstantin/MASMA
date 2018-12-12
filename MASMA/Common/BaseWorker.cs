using MASMA.Common.Models;
using MASMA.Message;

namespace MASMA.Common
{
    public class BaseWorker : BaseAgent
    {
        public BaseWorker() : base()
        {

        }

        public BaseWorker(string name) : base(name)
        {

        }

        public override void BeforeStop()
        {
            Send(Agents.MasterAgent, new BaseMessage<Statistic>
            {
                Message = new Message<Statistic>()
                {
                    Action = Actions.Statistic,
                    Data = this.Statistic
                }
            }.ToString());
        }
    }
}
