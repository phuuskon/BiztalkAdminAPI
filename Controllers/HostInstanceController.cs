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
    public class HostInstanceController : ControllerBase
    {
        /// <summary>
        /// Lists host instances in BizTalk server
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<HostInstance> Get()
        {
            List<HostInstance> hostInstances = new List<HostInstance>();
            try
            {
                //Create EnumerationOptions and run wql query 
                EnumerationOptions enumOptions = new EnumerationOptions();
                enumOptions.ReturnImmediately = false;

                //Search for all HostInstances 
                ManagementObjectSearcher searchObject = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select * from MSBTS_HostInstance", enumOptions);

                //Enumerate through the resultset 
                foreach (ManagementObject inst in searchObject.Get())
                {
                    HostInstance hostInstance = new HostInstance();
                    hostInstance.Caption = inst["Caption"]!=null?inst["Caption"].ToString():"";
                    hostInstance.ClusterInstanceType = inst["ClusterInstanceType"] != null ? inst["ClusterInstanceType"].ToString() : "";
                    hostInstance.ConfigurationState = inst["ConfigurationState"] != null ? inst["ConfigurationState"].ToString() : "";
                    hostInstance.Description = inst["Description"] != null ? inst["Description"].ToString() : "";
                    hostInstance.HostName = inst["HostName"] != null ? inst["HostName"].ToString() : "";
                    hostInstance.HostType = inst["HostType"] != null ? Enum.GetName(typeof(HostInstance.HostTypeEnum),inst["HostType"]) : "";
                    hostInstance.InstallDate = inst["InstallDate"] != null ? inst["InstallDate"].ToString() : "";
                    hostInstance.IsDisabled = inst["IsDisabled"] != null ? inst["IsDisabled"].ToString() : "";
                    hostInstance.Logon = inst["Logon"] != null ? inst["Logon"].ToString() : "";
                    hostInstance.MgmtDbNameOverride = inst["MgmtDbNameOverride"] != null ? inst["MgmtDbNameOverride"].ToString() : "";
                    hostInstance.MgmtDbServerOverride = inst["MgmtDbServerOverride"] != null ? inst["MgmtDbServerOverride"].ToString() : "";
                    hostInstance.Name = inst["Name"] != null ? inst["Name"].ToString() : "";
                    hostInstance.NTGroupName = inst["NTGroupName"] != null ? inst["NTGroupName"].ToString() : "";
                    hostInstance.RunningServer = inst["RunningServer"] != null ? inst["RunningServer"].ToString() : "";
                    hostInstance.ServiceState = inst["ServiceState"] != null ? Enum.GetName(typeof(HostInstance.ServiceStateEnum), inst["ServiceState"]) : "";
                    hostInstance.Status = inst["Status"] != null ? inst["Status"].ToString() : "";
                    hostInstance.UniqueID = inst["UniqueID"] != null ? inst["UniqueID"].ToString() : "";

                    hostInstances.Add(hostInstance);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occurred in get hostinstances call. " + ex.Message);
            }
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
