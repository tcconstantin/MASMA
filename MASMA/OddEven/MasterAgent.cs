using ActressMas;
using MASMA.Common;
using MASMA.Message;
using System;

namespace MASMA.OddEven
{
    public class MasterAgent: Agent
    {
        private int _phase = 0;
        private int _done = 0;
        private HighResTimer _counter = new HighResTimer();

        public override void BeforeStop()
        {
            Console.WriteLine(_counter.Stop());
            Utils.Assert(Utils.Source);
        }

        public override void Setup()
        {
            _counter.Start();

            BroadcastAll(new BaseMessage
            {
                Action = Actions.Start
            }.ToString());
        }

        public override void Act(ActressMas.Message message)
        {
            var baseMessage = new BaseMessage(message.Content);

            switch(baseMessage.Action)
            {
                case Actions.Done:
                    Broadcast(message.Sender, new BaseMessage { Action = Actions.Restart }.ToString());
                    _phase++;
                    break;
                case Actions.IsSorted:
                    _done++;

                    if(_done == 2)
                    {
                        Stop();
                    }

                    break;
            }
        }
    }
}
