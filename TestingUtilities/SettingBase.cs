using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingUtilities
{
    public class SettingBase
    {
        public TimeSpan PageTimeout { get; set; }
        public TimeSpan PollInterval { get; set; }
        public int MaxPollCount => Convert.ToInt32(Math.Round(PageTimeout.TotalMilliseconds / PollInterval.TotalMilliseconds, 0));
    }
}
