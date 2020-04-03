using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using BiztalkAdminAPI.Models;
using Microsoft.AspNetCore.Mvc;
using BiztalkAdminAPI.Services;

namespace BiztalkAdminAPI.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class HostInstanceController : ControllerBase
    {
        private readonly IHostInstanceService _service;

        public HostInstanceController(IHostInstanceService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// Lists host instances in BizTalk server
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<HostInstance> Get()
        {
            var hostInstances = _service.GetHostInstances();
                        
            return hostInstances;
        }

        /// <summary>
        /// Start or stop host instance.
        /// </summary>
        /// <remarks>
        /// Sample requests:
        ///  PUT /BizTalkAdminAPI/HostInstance/servername/hostinstance/Start
        ///  PUT /BizTalkAdminAPI/HostInstance/servername/hostinstance/Stop
        /// </remarks>
        /// <param name="servername"></param>
        /// <param name="hostname"></param>
        /// <param name="state"></param>
        [HttpPut("{servername}/{hostname}/{state}")]
        public void Put(string servername, string hostname, string state)
        {
            if (state == "Start" || state == "Stop")
            {
                EnumerationOptions enumOptions = new EnumerationOptions();
                enumOptions.ReturnImmediately = false;

                ManagementObjectSearcher searchObject = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select * from MSBTS_HostInstance Where RunningServer='" + servername + "' And HostName='" + hostname + "'", enumOptions);

                if (searchObject.Get().Count > 0)
                {
                    ManagementObjectCollection instcol = searchObject.Get();
                    ManagementObject inst = instcol.OfType<ManagementObject>().FirstOrDefault();

                    inst.InvokeMethod(state, null);
                }
            }
        }

        
    }
}
