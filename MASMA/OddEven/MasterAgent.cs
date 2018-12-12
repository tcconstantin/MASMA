using ActressMas;
using MASMA.Common;
using MASMA.Common.Models;
using MASMA.Message;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MASMA.OddEven
{
    public class MasterAgent : BaseMaster
    {
        private int _countSort = 0;
        private int _countPrint = 0;

        public MasterAgent() : 
            base(MASMA.Agents.MasterAgent)
        {

        }

        public override void BeforeStop()
        {
            base.BeforeStop();

            Utils.Assert(Utils.Destination);
        }

        public override void Setup()
        {
            Split();
        }

        public override void Act(ActressMas.Message message)
        {
            var baseMessage = new BaseMessage<object>(message.Content);

            switch (baseMessage.Message.Action)
            {
                case Actions.DoneAssignAndSort:

                    _countSort++;

                    if (_countSort == Utils.numAg)
                    {

                        int k = Utils.numAg - 1;
                        while (k > 0)
                        {
                            EvenPhase();
                            OddPhase();
                            k--;
                        }

                        for (int j = 0; j < Utils.agentPool.Count; j++)
                        {
                            Send(Utils.agentPool[j], new BaseMessage<int>
                            {
                                Message = new Message<int>()
                                {
                                    Action = Actions.PrintData
                                }
                            }.ToString());
                        }
                    }

                    break;
                case Actions.DonePrint:

                    _countPrint++;

                    if (_countPrint == Utils.numAg)
                    {
                        Utils.Destination = Utils.lsort.ToArray();
                        Stop();
                    }

                    break;
                case MASMA.Actions.Statistic:
                    Statistics.Add(JObject.Parse(baseMessage.Message.Data.ToString()).ToObject<Statistic>());
                    break;
            }
        }

        private void Split()
        {
            Pair pair = new Pair(0, 0);
            int agentIndex = 0;

            int blockSize = (int)decimal.Round((decimal)Utils.Source.Length / Utils.numAg);

            while (agentIndex < Utils.numAg)
            {
                pair.Start = blockSize * agentIndex;

                if ((Utils.numAg - 1 == agentIndex) ||
                    (pair.Start + blockSize > Utils.Source.Length))
                {
                    pair.End = Utils.Source.Length;
                }
                else
                {
                    pair.End = (agentIndex + 1) * blockSize;
                }

                Send(Utils.agentPool[agentIndex], new BaseMessage<List<int>>
                {
                    Message = new Message<List<int>>()
                    {
                        Action = Actions.AssignAndSort,
                        Data = this.RetreiveData(pair)
                    }
                }.ToString());

                agentIndex++;
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

        private bool HasNeighbor(string agent)
        {
            bool ret = false;
            int index = Utils.agentPool.IndexOf(agent);

            try
            {
                Utils.agentPool[index + 1].ToString();
                ret = true;
            }
            catch (Exception e)
            {
                ret = false;
            }

            return ret;
        }

        private void EvenPhase()
        {
            for (int i = 0; i < Utils.agentPool.Count; i++)
            {
                if (i % 2 == 0)
                {
                    if (HasNeighbor(Utils.agentPool[i]))
                    {
                        Send(Utils.agentPool[i], new BaseMessage<int>
                        {
                            Message = new Message<int>()
                            {
                                Action = Actions.EvenPhase
                            }
                        }.ToString());
                    }
                }
            }
        }

        private void OddPhase()
        {
            for (int i = 0; i < Utils.agentPool.Count; i++)
            {
                if (i % 2 == 1)
                {
                    if (HasNeighbor(Utils.agentPool[i]))
                    {
                        Send(Utils.agentPool[i], new BaseMessage<int>
                        {
                            Message = new Message<int>()
                            {
                                Action = Actions.OddPhase
                            }
                        }.ToString());
                    }
                }
            }
        }
    }
}
