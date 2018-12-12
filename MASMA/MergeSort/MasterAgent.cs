using MASMA.Common;
using MASMA.Common.Models;
using MASMA.Message;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MASMA.MergeSort
{
    public class MasterAgent: BaseMaster
    {
        private int _doneAssignData = 0;
        private int _doneSplit = 0;

        public MasterAgent() : base(Agents.MasterAgent)
        {

        }

        public override void Setup()
        {
            Send(Utils.AgentPool[0], new BaseMessage<List<int>>
            {
                Message = new Message<List<int>>
                {
                    Action = Actions.Split,
                    Data = Utils.Source.ToList()
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
                case Actions.Statistic:
                    Statistics.Add(JObject.Parse(baseMessage.Message.Data.ToString()).ToObject<Statistic>());
                    break;

                case Actions.DonePhase1:
                    _doneSplit++;

                    if (_doneSplit == Utils.ValidagentPool.Count)
                    {
                        //remove invalid workers(Nodes)
                        for (int j = 0; j < Utils.AgentPool.Count; j++)
                        {
                            if (!Utils.ValidagentPool.Contains(Utils.AgentPool[j]))
                            {
                                Send(Utils.AgentPool[j], new BaseMessage<int>
                                {
                                    Message = new Message<int>
                                    {
                                        Action = Actions.Terminate2
                                    }
                                }.ToString());
                            }
                        }

                        Utils.AgentPool = Utils.ValidagentPool;

                        for (int j = 0; j < Utils.ValidagentPool.Count; j++)
                        {
                            Send(Utils.ValidagentPool[j], new BaseMessage<int>
                            {
                                Message = new Message<int>
                                {
                                    Action = Actions.SortAndAssignData
                                }
                            }.ToString());
                        }
                    }

                    break;
                case Actions.DoneSortAndAssignData:
                    _doneAssignData++;

                    if (_doneAssignData == Utils.AgentPool.Count)
                    {
                        while (Utils.AgentPool.Count > 1)
                        {
                            MergeAndCompare();

                        }

                        Send(Utils.AgentPool[0], new BaseMessage<int>
                        {
                            Message = new Message<int>
                            {
                                Action = Actions.Print
                            }
                        }.ToString());

                        Stop();
                    }
                    break;

            }
        }

        private void MergeAndCompare()
        {
            if (Utils.AgentPool.Count > 1)
            {
                for (int i = 1; i < Utils.AgentPool.Count; i += 2)
                {
                    Send(Utils.AgentPool[i++], new BaseMessage<int>
                    {
                        Message = new Message<int>
                        {
                            Action = Actions.MergeAndCompare
                        }
                    }.ToString());
                }
            }
        }

        private List<int> RetreiveData(Pair pair)
        {
            List<int> ret = new List<int>();

            for (int i = pair.Start; i < pair.End; i++)
            {
                ret.Add(Utils.Source[i]);
            }

            return ret;
        }

        private void Split()
        {
            int agentIndex = 0;
            Pair pair = new Pair(0, 0);

            int blockSize = (int)decimal.Round((decimal)Utils.Source.Length / Utils.NrAgent);

            while (agentIndex < Utils.NrAgent)
            {
                pair.Start = blockSize * agentIndex;

                if (Utils.NrAgent - 1 == agentIndex || pair.Start + blockSize > Utils.Source.Length)
                {
                    pair.End = Utils.Source.Length;
                }
                else
                {
                    pair.End = (agentIndex + 1) * blockSize;
                }

                if (pair.End - pair.Start < 1)
                {
                    Send(Utils.AgentPool[agentIndex], new BaseMessage<int>
                    {
                        Message = new Message<int>
                        {
                            Action = Actions.Terminate
                        }

                    }.ToString());
                }
                else
                {
                    Send(Utils.AgentPool[agentIndex], new BaseMessage<List<int>>
                    {
                        Message = new Message<List<int>>
                        {
                            Action = Actions.SortAndAssignData,
                            Data = RetreiveData(pair)
                        }
                    }.ToString());
                }

                agentIndex++;
            }
        }
    }
}
