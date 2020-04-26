using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BiztalkAdminAPI.Models;
using System.Management;
using System.Data.SqlClient;

namespace BiztalkAdminAPI.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class OrchestrationController : ControllerBase
    {
        /// <summary>
        /// Lists orchestrations in BizTalk server.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Orchestration> Get()
        {
            List<Orchestration> orchestrations = new List<Orchestration>();
            try
            {
                //Create EnumerationOptions and run wql query 
                EnumerationOptions enumOptions = new EnumerationOptions();
                enumOptions.ReturnImmediately = false;

                //Search for DB servername and trackingDB name 
                ManagementObjectSearcher searchObject = new ManagementObjectSearcher("root\\MicrosoftBizTalkServer", "Select TrackingDBServerName, TrackingDBName from MSBTS_GroupSetting", enumOptions);
                ManagementObjectCollection searchCollection = searchObject.Get();
                ManagementObject obj = searchCollection.OfType<ManagementObject>().FirstOrDefault();

                string connectionString = "Server=" + obj["TrackingDBServerName"] + ";Database=" + obj["TrackingDBName"] + ";Integrated Security=True;";
                string query = @"select A.nvcName [ApplicationName] " +
                                ", O.[nvcFullName] [OrchestrationName] " +
                                ", O.nOrchestrationStatus [OrchestrationStatus] " +
                                ", MAX(SF.[ServiceInstance/StartTime]) [LastStartDateTime] " +
                                "from BizTalkMgmtDb.dbo.bts_orchestration O " +
                                "left outer join dtav_ServiceFacts SF on SF.[Service/Name] = O.nvcFullName " +
                                "join BizTalkMgmtDb.dbo.bts_item I on I.id = O.nItemID " +
                                "join BizTalkMgmtDb.dbo.bts_assembly ASSY on ASSY.nID = I.AssemblyId " +
                                "join BizTalkMgmtDb.dbo.bts_application A on  A.nID = ASSY.nApplicationID " +
                                "group by A.nvcName, O.[nvcFullName], O.nOrchestrationStatus;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            Orchestration orchestration = new Orchestration();
                            orchestration.ApplicationName = (string)reader["ApplicationName"];
                            orchestration.OrchestrationName = (string)reader["OrchestrationName"];
                            orchestration.OrchestrationStatus = Enum.GetName(typeof(Orchestration.OrchestrationStatusEnum), (int)reader["OrchestrationStatus"]);
                            if (reader["LastStartDateTime"] != DBNull.Value)
                                orchestration.LastStartDateTime = (DateTime)reader["LastStartDateTime"];

                            orchestrations.Add(orchestration);
                        }
                    }
                    finally
                    {
                        // Always call Close when done reading.
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occurred in get orchestrations call. " + ex.Message);
            }
            return orchestrations;
        }
    }
}