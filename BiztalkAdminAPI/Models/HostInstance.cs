namespace BiztalkAdminAPI.Models
{
    public class HostInstance
    {
        public string Caption { get; set; } = "";
        public string ClusterInstanceType { get; set; } = "";
        public string ConfigurationState { get; set; } = "";
        public string Description { get; set; } = "";
        public string HostName { get; set; } = "";
        public string HostType { get; set; } = "";
        public string InstallDate { get; set; } = "";
        public string IsDisabled { get; set; } = "";
        public string Logon { get; set; } = "";
        public string MgmtDbNameOverride { get; set; } = "";
        public string MgmtDbServerOverride { get; set; } = "";
        public string Name { get; set; } = "";
        public string NTGroupName { get; set; } = "";
        public string RunningServer { get; set; } = "";
        public string ServiceState { get; set; } = "";
        public string Status { get; set; } = "";
        public string UniqueID { get; set; } = "";

        public enum ServiceStates
        {
            Stopped = 1,
            Start_pending = 2,
            Stop_pending = 3,
            Running = 4,
            Continue_pending = 5,
            Pause_pending = 6,
            Paused = 7,
            Unknown = 8
        }

        public enum HostTypes
        {
            In_process = 1,
            Isolated = 2
        }
    }
}
