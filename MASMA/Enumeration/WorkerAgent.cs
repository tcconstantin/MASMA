using MASMA.Common;
using MASMA.Common.Models;
using MASMA.Message;

namespace MASMA.Enumeration
{
    public class WorkerAgent: BaseWorker
    {
        private int _agentIndex;

        public WorkerAgent(int index) : base($"Worker-{index}")
        {
            _agentIndex = index;
        }

        public override void Act(ActressMas.Message message)
        {
            var baseMessage = new BaseMessage<int>(message.Content);

            switch (baseMessage.Message.Action)
            {
                case Actions.Sort:

                    if (Utils.SkipNext)
                    {
                       this.StopProcess();
                    }

                    var pair = this.GetIndexes();

                    if (pair.End == Utils.Length)
                    {
                        Utils.SkipNext = true;
                    }

                    this.HandleSort(pair);
                    this.StopProcess();

                    break;
            }
        }

        private Pair GetIndexes()
        {
            int startIndex = 0;
            int endIndex = 0;

            int blockSize = (int)decimal.Round((decimal)Utils.Length / Utils.NoAgents);
            startIndex = blockSize * _agentIndex;

            if (((Utils.NoAgents - 1) == _agentIndex) ||
                ((startIndex + blockSize) > Utils.Length))
            {
                endIndex = Utils.Length;
            }
            else
            {
                endIndex = (_agentIndex + 1) * blockSize;
            }

            return new Pair(startIndex, endIndex);
        }

        private void HandleSort(Pair pair)
        {
            for (int i = pair.Start; i < pair.End; i++)
            {
                int value = Utils.Source[i];
                int rank = 0;

                for (int j = 0; j < Utils.Length; j++)
                {
                    if (value > Utils.Source[j])
                    {
                        rank++;
                    }

                    if (value == Utils.Source[j] && (i < j))
                    {
                        rank++;
                    }
                }

                Utils.Destination[rank] = value;

                Statistic.NoAtomicOperation++;
            }
        }

        private void StopProcess()
        {
            Send(Agents.MasterAgent, new BaseMessage<int>
            {
               Message = new Message<int>
               {
                   Action = Actions.Done
               }
            }.ToString());

            Stop();
        }
    }
}
