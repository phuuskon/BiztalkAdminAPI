using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiztalkAdminAPI.Models
{
    public class BizTalkServerStatus
    {
        public int HostInstancesRunning { get; set; }
        public int HostInstancesNotRunning { get; set; }
        public int ReceiveLocationsEnabled { get; set; }
        public int ReceiveLocationsDisabled { get; set; }
        public int SuspendedInstancesCount { get; set; }
        public int RunningInstancesCount { get; set; }
    }
}
