using MASMA.Common;
using MASMA.Message;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MASMA.MergeSort
{
    public class WorkerAgent: BaseWorker
    {
        private List<int> _data;
        private int _doneAssign = 0;

        public WorkerAgent() : base($"Worker-{new Random().Next(1000)}")
        {
            _data = Enumerable.Repeat(0, Utils.Length).ToList();
        }


        public override void Act(ActressMas.Message message)
        {
            var baseMessage = new BaseMessage<object>(message.Content);

            switch (baseMessage.Message.Action)
                {
                    case Actions.DoneAsignData:
                        _doneAssign++;
                        // first split [L,R]

                        if (_doneAssign == 2)
                        {
                            while (!Utils.finalSplit)
                            {
                                for (int i = 1; i < Utils.AgentPool.Count; i += 1)
                                {
                                    Send(Utils.AgentPool[i], new BaseMessage<int>
                                    {
                                        Message = new Message<int>
                                        {
                                            Action = Actions.OwnSplit
                                        }
                                    }.ToString());
                                }
                            }
                        }

                        break;

                    case Actions.OwnSplit:
                        var sp1_own = _data.Take((_data.Count + 1) / 2).ToList();
                        var sp2_own = _data.Skip((_data.Count + 1) / 2).ToList();

                        if (sp1_own.Count >= Utils.Threshold && sp2_own.Count >= Utils.Threshold)
                        {
                            int a = CreateWorker(Utils.AgentPool.Count);
                            int b = CreateWorker(Utils.AgentPool.Count);
                            Thread.Sleep(200);

                            Send(Utils.AgentPool[a], new BaseMessage<List<int>>
                            {
                                Message = new Message<List<int>>
                                {
                                    Action = Actions.Assign,
                                    Data = sp1_own
                                }
                                
                            }.ToString());

                        Send(Utils.AgentPool[b], new BaseMessage<List<int>>
                        {
                            Message = new Message<List<int>>
                            {
                                Data = sp2_own,
                                Action = Actions.Assign
                            }
                        }.ToString());
                        }
                        else
                        {
                            if (this._data.Count >= Utils.Threshold)
                                Utils.ValidagentPool.Add(this.Name);

                            Utils.finalSplit = true;
                            Send(Agents.MasterAgent, new BaseMessage<string>
                            {
                                Message = new Message<string>
                                {
                                    Action = Actions.DonePhase1,
                                    Data = this.Name
                                }
                                
                            }.ToString());
                        }
                        break;
                    //main split from [Worker00]
                    case Actions.Split:

                        var toSplit =  JArray.Parse(baseMessage.Message.Data.ToString()).ToObject<List<int>>();
                        var sp1 = toSplit.Take((toSplit.Count + 1) / 2).ToList();
                        var sp2 = toSplit.Skip((toSplit.Count + 1) / 2).ToList();
                    if (sp1.Count >= 2 && sp2.Count >= 2)
                    {
                        CreateWorker(Utils.AgentPool.Count);
                        CreateWorker(Utils.AgentPool.Count);
                        Thread.Sleep(200);

                        Send(Utils.AgentPool[1], new BaseMessage<List<int>>
                        {
                            Message = new Message<List<int>>
                            {
                                Action = Actions.AssignData,
                                Data = sp1
                            }

                        }.ToString());

                        Send(Utils.AgentPool[2], new BaseMessage<List<int>>
                        {
                            Message = new Message<List<int>>
                            {
                                Action = Actions.AssignData,
                                Data = sp2
                            }

                        }.ToString());
                    }
                        break;
                    case Actions.Assign:
                    _data = JArray.Parse(baseMessage.Message.Data.ToString()).ToObject<List<int>>();
                    break;
                    case Actions.AssignData:
                        _data = JArray.Parse(baseMessage.Message.Data.ToString()).ToObject<List<int>>();

                    Send(Utils.AgentPool[0], new BaseMessage<int>
                        {
                            Message = new Message<int>
                            {
                                Action = Actions.DoneAsignData
                            }
                        }.ToString());

                        break;

                    case Actions.SortAndAssignData:
                    _data.Sort();
                        Send(Agents.MasterAgent, new BaseMessage<int>
                        {
                            Message = new Message<int>
                            {
                                Action = Actions.DoneSortAndAssignData
                            }
                        }.ToString());
                        break;
                    //i+1
                    case Actions.MergeAndCompare:
                        //send i -> data  ->merge
                        int index = Utils.AgentPool.IndexOf(this.Name);
                    if (index != 0 && index != Utils.NrAgent)
                    {
                        Send(Utils.AgentPool[index - 1], new BaseMessage<List<int>>
                        {
                            Message = new Message<List<int>>
                            {
                                Data = this._data,
                                Action = Actions.Merge
                            }
                        }.ToString());
                    }
                        Utils.AgentPool.Remove(this.Name);
                        Stop();
                        //stop
                        break;
                    case Actions.Merge:

                    var received = JArray.Parse(baseMessage.Message.Data.ToString()).ToObject<List<int>>();
                    var own = this._data;
                        var result = received
                             .Concat(own)
                             .OrderBy(x => x)
                             .ToList();
                        this._data = result;

                        break;
                    case Actions.Print:
                        for (int i = 0; i < _data.Count; i++)
                        {
                            Utils.Destination[i] = _data[i];
                        }
                        Stop();

                        break;
                    case Actions.Terminate:
                        Utils.AgentPool.Remove(this.Name);
                        Stop();
                        break;
                    //stop - without removing from agentPool
                    case Actions.Terminate2:
                        Stop();
                        break;
                }

        }

        private int CreateWorker(int index)
        {
            var worker = new WorkerAgent();

            Utils.Env.Add(worker, string.Format("Worker-{0:D2}", index));
            Utils.AgentPool.Add(worker.Name);
            worker.Start();

            return index;
        }


    }
}
