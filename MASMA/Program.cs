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

//#define ENUMERATION

#if MERGESORT
using MASMA.MergeSort;
#elif ENUMERATION
using MASMA.Enumeration;
#else
using MASMA.OddEven;
#endif

namespace MASMA
{
    class Program
    {
        static void Main(string[] args)
        {

            Utils.Init(Case.WORST);

            var env = new ActressMas.Environment();

#if MERGESORT
            var masterAgent = new MasterAgent();
            var leftAgent = new WorkerAgent();
            var rightAgent = new WorkerAgent();

            env.Add(leftAgent, Agents.WorkerAgentLeft);
            leftAgent.Start();

            env.Add(rightAgent, Agents.WorkerAgentRight);
            rightAgent.Start();

            env.Add(masterAgent, Agents.MasterAgent);
            masterAgent.Start();
#elif ENUMERATION
            for (int i = 0; i < Utils.NoAgents; i++)
            {
                var workerAgent = new WorkerAgent(i);
                env.Add(workerAgent, string.Format("Slave{0:D2}", i));
                workerAgent.Start();
            }

            var masterAgent = new MasterAgent();
            env.Add(masterAgent, Agents.MasterAgent);
            masterAgent.Start();

#else
            for (int i = 0; i < 2; i++)
            {
                var workerAgent = new WorkerAgent(i);
                env.Add(workerAgent, string.Format("Slave{0:D2}", i));
                workerAgent.Start();
            }

            var masterAgent = new MasterAgent();
            env.Add(masterAgent, Agents.MasterAgent);
            masterAgent.Start();

#endif
            env.WaitAll();

        }
    }
}
