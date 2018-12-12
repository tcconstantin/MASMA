using MASMA.Common;
using MASMA.Message;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MASMA.OddEven
{
    public class WorkerAgent : BaseWorker
    {
        private List<int> _data;

        public WorkerAgent(int id) : base($"Worker-{id}")
        {
            _data = Enumerable.Repeat(0, Utils.Length).ToList();
        }

        public override void Act(ActressMas.Message message)
        {
            var baseMessage = new BaseMessage<object>(message.Content);

            switch (baseMessage.Message.Action)
            {
                case Actions.AssignAndSort:

                    AsignData(JArray.Parse(baseMessage.Message.Data.ToString()).ToObject<List<int>>());
                    Sort();

                    Send(Agents.MasterAgent, new BaseMessage<int>
                    {
                        Message =  new Message<int>()
                        {
                            Action = Actions.DoneAssignAndSort
                        }
                    }.ToString());

                    break;

                case Actions.EvenPhase:

                    if (HasNeighbor(this.Name))
                    {
                        int index = Utils.agentPool.IndexOf(this.Name);

                        Send(Utils.agentPool[index + 1], new BaseMessage<List<int>>
                        {
                            Message = new Message<List<int>>()
                            {
                                Action = Actions.MergeAndSplit,
                                Data = this._data
                            }
                            
                        }.ToString());
                    }

                    break;
                case Actions.OddPhase:

                    if (HasNeighbor(this.Name))
                    {
                        int index = Utils.agentPool.IndexOf(this.Name);

                        Send(Utils.agentPool[index + 1], new BaseMessage<List<int>>
                        {
                            Message = new Message<List<int>>()
                            {
                                Action = Actions.MergeAndSplit,
                                Data =  _data
                            }
                        }.ToString());
                    }

                    break;

                case Actions.MergeAndSplit:

                    List<int> received = JArray.Parse(baseMessage.Message.Data.ToString()).ToObject<List<int>>();
                    int[] own = this._data.ToArray();

                    int[] result = received
                         .Concat(own)
                         .OrderBy(x => x)
                         .ToArray();

                    List<int> sendBack = result.Take((result.Length + 1) / 2).ToList();
                    this._data = result.Skip((result.Length + 1) / 2).ToList();

                    Send(message.Sender, new BaseMessage<List<int>>
                    {
                        Message = new Message<List<int>>()
                        {
                            Action = Actions.RefreshData,
                            Data = sendBack
                        } 
                    }.ToString());

                    break;
                case Actions.RefreshData:
                      
                     AsignData(JArray.Parse(baseMessage.Message.Data.ToString()).ToObject<List<int>>());

                    break;

                case Actions.PrintData:

                    for (int i = 0; i < _data.Count; i++)
                    {
                        Utils.lsort.Add(_data[i]);
                    }

                    Utils.destination.Add(this.Name, this._data.ToArray());

                    Send(Agents.MasterAgent, new BaseMessage<int>
                    {
                        Message = new Message<int>()
                        {
                            Action = Actions.DonePrint
                        }
                    }.ToString());

                    Stop();

                    break;
            }
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

        private void AsignData(List<int> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                this._data[i] = data[i];
            }
        }

        private void Sort()
        {
            _data.Sort();
        }
    }
}
