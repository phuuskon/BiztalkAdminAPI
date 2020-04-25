using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiztalkAdminAPI.Models
{
    public class SendPort
    {
        public string ApplicationName { get; set; }
        public string SendPortName { get; set; }
        public string PortStatus { get; set; }
        public DateTime? LastMessageSentDateTime { get; set; }

        public enum SendPortStatusEnum
        {
            None = 0,
            UnEnlisted = 1,
            Stopped = 2,
            Started = 3
        }
    }
}
