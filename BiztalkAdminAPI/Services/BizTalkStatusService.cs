using BiztalkAdminAPI.Models;
using System.Management;

namespace BiztalkAdminAPI.Services
{
    public class BizTalkStatusService : IBizTalkStatusService
    {
        public BizTalkServerStatus GetBizTalkServerStatus()
        {
            BizTalkServerStatus BtsStatus = new BizTalkServerStatus();
            
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

            return BtsStatus;
        }
    }

    public interface IBizTalkStatusService
    {
        BizTalkServerStatus GetBizTalkServerStatus();
    }
}