using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiztalkAdminAPI.Models
{
    public class Orchestration
    {
        public string ApplicationName { get; set; }
        public string OrchestrationName { get; set; }
        public string OrchestrationStatus { get; set; }
        public DateTime? LastStartDateTime { get; set; }

        public enum OrchestrationStatusEnum
        {
            None = 0,
            UnEnlisted = 1,
            Stopped = 2,
            Started = 3
        }
    }
}
