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
    public class ReceiveLocationController : ControllerBase
    {
        /// <summary>
        /// Lists receivelocations in BizTalk server.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ReceiveLocation> Get()
        {
            List<ReceiveLocation> receiveLocations = new List<ReceiveLocation>();
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
                string query = @"select A.nvcName [ApplicationName]" +
                                ",RP.nvcName [ReceivePortName] " +
                                ",RL.Name [ReceiveLocationName] " +
                                ",RL.Disabled [Disabled] " +
                                ",MAX(MF.[Event/Timestamp]) [LastMessageReceivedDateTime] " +
                                "FROM " +
                                "BizTalkMgmtDb.dbo.adm_ReceiveLocation RL " +
                                "JOIN BizTalkMgmtDb.dbo.bts_receiveport RP ON RP.nID = RL.ReceivePortId " +
                                "LEFT OUTER JOIN BizTalkDTADb.dbo.dtav_MessageFacts MF ON MF.[Event/Port] = RP.nvcName " +
                                "AND MF.[Event/URL] = RL.InboundTransportURL " +
                                "JOIN BizTalkMgmtDb.dbo.bts_application A ON RP.nApplicationID = A.nID " +
                                "group by A.nvcName, RP.nvcName, RL.Name, RL.Disabled;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            ReceiveLocation receiveLocation = new ReceiveLocation();
                            receiveLocation.ApplicationName = (string)reader["ApplicationName"];
                            receiveLocation.ReceivePortName = (string)reader["ReceivePortName"];
                            receiveLocation.ReceiveLocationName = (string)reader["ReceiveLocationName"];
                            receiveLocation.Disabled = (int)reader["Disabled"] == 0 ? false : true;
                            if(reader["LastMessageReceivedDateTime"] != DBNull.Value)
                                receiveLocation.LastMessageReceivedDateTime = (DateTime)reader["LastMessageReceivedDateTime"];

                            receiveLocations.Add(receiveLocation);
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
                throw new Exception("Exception Occurred in get receivelocations call. " + ex.Message);
            }
            return receiveLocations;
        }
    }
}