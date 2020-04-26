using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using BiztalkAdminAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BiztalkAdminAPI.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class BizTalkStatusController : ControllerBase
    {
        /// <summary>
        /// Gets overall status of BizTalk Server
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public BizTalkServerStatus Get()
        {
            BizTalkServerStatus BtsStatus = new BizTalkServerStatus();
            try
            {
                //Create EnumerationOptions and run wql query 
                EnumerationOptions enumOptions = new EnumerationOptions();
                enumOptions.ReturnImmediately = false;

                //Search count of running HostInstances 
                ManagementObjectSearcher searchObject = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select * from MSBTS_HostInstance where ServiceState=4 and HostType=1", enumOptions);
                BtsStatus.HostInstancesRunning = searchObject.Get().Count;

                //Search count of not running HostInstances 
                searchObject = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select * from MSBTS_HostInstance where ServiceState != 4 and HostType = 1", enumOptions);
                BtsStatus.HostInstancesNotRunning = searchObject.Get().Count;

                //Search count of not Rlocs enabled 
                searchObject = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select * from MSBTS_ReceiveLocation where IsDisabled = false", enumOptions);
                BtsStatus.ReceiveLocationsEnabled = searchObject.Get().Count;

                //Search count of not Rlocs disabled 
                searchObject = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select * from MSBTS_ReceiveLocation where IsDisabled = true", enumOptions);
                BtsStatus.ReceiveLocationsDisabled = searchObject.Get().Count;

                // Search count of suspended instances
                searchObject = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select * from MSBTS_ServiceInstance Where (ServiceClass=1 or ServiceClass=4) and (ServiceStatus=4 or ServiceStatus=32)", enumOptions);
                BtsStatus.SuspendedInstancesCount = searchObject.Get().Count;

            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occurred in get btsStatus call. " + ex.Message);
            }
            return BtsStatus;
        }
    }
}