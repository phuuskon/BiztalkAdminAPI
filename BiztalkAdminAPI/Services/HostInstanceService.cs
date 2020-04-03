using BiztalkAdminAPI.Models;
using System.Management;
using System.Collections.Generic;
using System;

namespace BiztalkAdminAPI.Services
{
    public class HostInstanceService : IHostInstanceService
    {
        public IEnumerable<HostInstance> GetHostInstances()
        {
            List<HostInstance> hostInstances = new List<HostInstance>();
            
            //Create EnumerationOptions and run wql query 
            EnumerationOptions enumOptions = new EnumerationOptions();
            enumOptions.ReturnImmediately = false;

            //Search for all HostInstances 
            ManagementObjectSearcher searchObject = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select * from MSBTS_HostInstance", enumOptions);

            //Enumerate through the resultset 
            foreach (ManagementBaseObject inst in searchObject.Get())
            {
                HostInstance hostInstance = new HostInstance();
                hostInstance.Caption = inst["Caption"].ToString();
                hostInstance.ClusterInstanceType = inst["ClusterInstanceType"].ToString();
                hostInstance.ConfigurationState = inst["ConfigurationState"].ToString();
                hostInstance.Description = inst["Description"].ToString();
                hostInstance.HostName = inst["HostName"].ToString();
                hostInstance.HostType = Enum.GetName(typeof(HostInstance.HostTypes),inst["HostType"]);
                hostInstance.InstallDate = inst["InstallDate"].ToString();
                hostInstance.IsDisabled = inst["IsDisabled"].ToString();
                hostInstance.Logon = inst["Logon"].ToString();
                hostInstance.MgmtDbNameOverride = inst["MgmtDbNameOverride"].ToString();
                hostInstance.MgmtDbServerOverride = inst["MgmtDbServerOverride"].ToString();
                hostInstance.Name = inst["Name"].ToString();
                hostInstance.NTGroupName = inst["NTGroupName"].ToString();
                hostInstance.RunningServer = inst["RunningServer"].ToString();
                hostInstance.ServiceState = Enum.GetName(typeof(HostInstance.ServiceStates), inst["ServiceState"]);
                hostInstance.Status = inst["Status"].ToString();
                hostInstance.UniqueID = inst["UniqueID"].ToString();

                hostInstances.Add(hostInstance);
            }
            return hostInstances;
        }
    }

    public interface IHostInstanceService
    {
        IEnumerable<HostInstance> GetHostInstances();
    }
}