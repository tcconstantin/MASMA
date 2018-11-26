/**************************************************************************
 *                                                                        *
 *  File:        Program.cs                                               *
 *  Description: Merge Sort multi-agent                                   *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/

using MASMA.Enumaration;

namespace MASMA
{
    class Program
    {
        static void Main(string[] args)
        {
#if false
            int len = Utils.Length;
            var source = new int[len];
            var dest = new int[len];

            for (int i = 0; i < source.Length; i++)
            {
                source[i] = len - i;
            }

            var counter = new HighResTimer();
            counter.Start();

            ParallelAlgorithm.SortMergeInnerPar(source, 0, source.Length - 1, dest, true);

            var time = counter.Stop();
            Console.WriteLine(time);



            for (int i = 0; i < source.Length - 1; ++i)
            {
                if (dest[i] > dest[i + 1])
                {
                    Console.WriteLine("crash");
                    break;
                }
            }

#else
           

            for (int i = 0; i < Utils.Length; i++)
            {
                Utils.Source[i] = Utils.Length - i;
            }

            var env = new ActressMas.Environment();

            //var masterAgent = new MasterAgent();
            //var leftAgent = new WorkerAgent();
            //var rightAgent = new WorkerAgent();

            //env.Add(leftAgent, Agents.WorkerAgentLeft);
            //leftAgent.Start();

            //env.Add(rightAgent, Agents.WorkerAgentRight);
            //rightAgent.Start();

            //env.Add(masterAgent, Agents.MasterAgent);
            //masterAgent.Start();

            for (int i = 0; i < Utils.NoAgents; i++)
            {
                var workerAgent = new WorkerAgent(i);
                env.Add(workerAgent, string.Format("Slave{0:D2}", i));
                workerAgent.Start();
            }

            var masterAgent = new MasterAgent();
            env.Add(masterAgent, Agents.MasterAgent);
            masterAgent.Start();

            env.WaitAll();
#endif
        }
    }
}
