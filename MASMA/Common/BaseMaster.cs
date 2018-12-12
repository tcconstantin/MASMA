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
            Program.Log.Debug($"[{name}]: Ctor");
        }

        public override void BeforeStop()
        {
            Statistics.Add(this.Statistic);

            Program.Log.Warn($"[{Name}]: Statistics:");

            foreach (var statistic in Statistics)
            {
                Program.Log.Warn(statistic.ToString());
            }
        }
    }
}
