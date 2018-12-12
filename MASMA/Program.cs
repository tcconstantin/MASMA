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
#define MERGESORT
using System;
using log4net;
using log4net.Config;
using System.Threading;


#if MERGESORT
using MASMA.MergeSort;
#elif ENUMERATION
using MASMA.Enumeration;
#else
using MASMA.OddEven;
using System.Threading;
#endif

namespace MASMA
{
    class Program
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            Utils.Init(Case.WORST);

            var env = new ActressMas.Environment();
            Utils.Env = new ActressMas.Environment();


#if MERGESORT
            var workerAgent = new WorkerAgent();

            Utils.Env.Add(workerAgent, string.Format("Slave{0:D2}", 0));
            Utils.AgentPool.Add(workerAgent.Name);
            workerAgent.Start();

            Thread.Sleep(200);

            var masterAgent = new MasterAgent();
            Utils.Env.Add(masterAgent, Agents.MasterAgent);
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
            Utils.NrAgent = 6;
            Utils.slaveElem = Utils.Source.Length / Utils.NrAgent;

            for (int i = 0; i < Utils.NrAgent; i++)
            {
                var workerAgent = new WorkerAgent(i);
                env.Add(workerAgent, string.Format("Slave{0:D2}", i));
                Utils.AgentPool.Add(workerAgent.Name);
                workerAgent.Start();
            }
            Thread.Sleep(1000);
            var masterAgent = new MasterAgent();
            env.Add(masterAgent, Agents.MasterAgent);
            masterAgent.Start();

#endif
            env.WaitAll();
            Console.ReadKey();
        }
    }
}
