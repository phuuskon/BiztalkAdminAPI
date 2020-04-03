using BiztalkAdminAPI.Services;
using BiztalkAdminAPI.Models;

namespace BiztalkAdminAPI.Tests
{
    public class BizTalkStatusServiceFake : IBizTalkStatusService
    {
        private readonly BizTalkServerStatus _btsStatus;

        public BizTalkStatusServiceFake()
        {
            _btsStatus = new BizTalkServerStatus();
            _btsStatus.HostInstancesRunning = 2;
            _btsStatus.HostInstancesNotRunning = 0;
            _btsStatus.ReceiveLocationsEnabled = 5;
            _btsStatus.ReceiveLocationsDisabled = 0;
            _btsStatus.SuspendedInstancesCount = 10;            
        }

        public BizTalkServerStatus GetBizTalkServerStatus()
        {
            return _btsStatus;
        }
    }
}