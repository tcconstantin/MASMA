using MASMA.Common.Models;
using MASMA.Message;
using System;
using System.Collections.Generic;

namespace MASMA.Common
{
    public class BaseMaster: BaseAgent
    {
        protected List<Statistic> Statistics = new List<Statistic>();

        public BaseMaster() : base()
        {

        }

        public BaseMaster(string name) : base(name)
        {

        }

        public override void BeforeStop()
        {
            Statistics.Add(this.Statistic);

            foreach (var statistic in Statistics)
            {
                Console.WriteLine(statistic.ToString());
            }
        }
    }
}
