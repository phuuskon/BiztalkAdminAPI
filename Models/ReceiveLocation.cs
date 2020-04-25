using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiztalkAdminAPI.Models
{
    public class ReceiveLocation
    {
        public string ApplicationName { get; set; }
        public string ReceivePortName { get; set; }
        public string ReceiveLocationName { get; set; }
        public bool Disabled { get; set; }
        public DateTime? LastMessageReceivedDateTime { get; set; }
    }
}
