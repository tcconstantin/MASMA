using ActressMas;
using MASMA.Message;
using System;

namespace MASMA.OddEven
{
    public class WorkerAgent: Agent
    {
        private int _phase = 0;
        private readonly int _id;

        public WorkerAgent(int id)
        {
            _id = id;
        }

        public override void Act(ActressMas.Message message)
        {
            var baseMessage = new BaseMessage(message.Content);

            switch(baseMessage.Action)
            {
                case Actions.Restart:
                case Actions.Start:
                    Sort();
                    break;
            }
        }

        private void Sort()
        {
            if (!Utils.IsSorted)
            {
                _phase++;

                // event agent
                if (_id % 2 == 0)
                {
                    Utils.IsSorted = true;
                    for (int i = 0; i <= Utils.Length - 2; i += 2)
                    {
                        if (Utils.OrderType)
                        {
                            if (Utils.Source[i] > Utils.Source[i + 1])
                            {
                                Utils.Swap(ref Utils.Source[i], ref Utils.Source[i + 1]);
                                Utils.IsSorted = false;
                            }
                        }
                        else
                        {
                            if (Utils.Source[i] < Utils.Source[i + 1])
                            {
                                Utils.Swap(ref Utils.Source[i], ref Utils.Source[i + 1]);
                                Utils.IsSorted = false;
                            }
                        }
                    }
                }
                else // odd agent
                {
                    Utils.IsSorted = true;
                    for (int i = 1; i <= Utils.Length - 2; i += 2)
                    {
                        if (Utils.OrderType)
                        {
                            if (Utils.Source[i] > Utils.Source[i + 1])
                            {
                                Utils.Swap(ref Utils.Source[i], ref Utils.Source[i + 1]);
                                Utils.IsSorted = false;
                            }
                        }
                        else
                        {
                            if (Utils.Source[i] < Utils.Source[i + 1])
                            {
                                Utils.Swap(ref Utils.Source[i], ref Utils.Source[i + 1]);
                                Utils.IsSorted = false;
                            }
                        }
                    }
                }

                Console.WriteLine("Agent [" + _id + "]  ---> round " + this._phase);

                Send(Agents.MasterAgent, new BaseMessage
                {
                    Action = Actions.Done
                }.ToString());
            }
            else
            {
                Send(Agents.MasterAgent, new BaseMessage
                {
                    Action = Actions.IsSorted
                }.ToString());

                Stop();
            }
        }
    }
}
