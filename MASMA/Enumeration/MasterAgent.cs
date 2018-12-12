using MASMA.Common;
using MASMA.Common.Models;
using MASMA.Message;
using Newtonsoft.Json.Linq;

namespace MASMA.Enumeration
{
    public class MasterAgent: BaseMaster
    {
        private int _done;

        public MasterAgent() : base(Agents.MasterAgent)
        {
            _done = 0;
        }

        public override void Setup()
        {
            BroadcastAll(new BaseMessage<int>
            {
                Message = new Message<int>()
                {
                    Action = Actions.Sort
                }
            }.ToString());
        }

        public override void BeforeStop()
        {
            base.BeforeStop();

            Utils.Assert(Utils.Destination);
        }

        public override void Act(ActressMas.Message message)
        {
            var baseMessage = new BaseMessage<object>(message.Content);

            switch (baseMessage.Message.Action)
            {
                case Actions.Done:
                    _done++;

                    if (_done == Utils.NoAgents)
                        Stop();

                    break;
                case Actions.Statistic:
                    Statistics.Add(JObject.Parse(baseMessage.Message.Data.ToString()).ToObject<Statistic>());
                    break;
            }
        }
    }
}
