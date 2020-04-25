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
    public class SendPortController : ControllerBase
    {
        /// <summary>
        /// Lists sendports in BizTalk server.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<SendPort> Get()
        {
            List<SendPort> sendPorts = new List<SendPort>();
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
                                ",SP.nvcName [SendPortName] " +
                                ",SP.nPortStatus [PortStatus] " +
                                ",MAX(MF.[Event/Timestamp]) [LastMessageSentDateTime] " +
                                "FROM " +
                                "BizTalkMgmtDb.dbo.bts_sendport SP " +
                                "LEFT OUTER JOIN BizTalkDTADb.dbo.dtav_MessageFacts MF ON MF.[Event/Port] = SP.nvcName " +
                                "JOIN BizTalkMgmtDb.dbo.bts_application A ON SP.nApplicationID = A.nID " +
                                "group by A.nvcName, SP.nvcName, SP.nPortStatus;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            SendPort sendPort = new SendPort();
                            sendPort.ApplicationName = (string)reader["ApplicationName"];
                            sendPort.SendPortName = (string)reader["SendPortName"];
                            sendPort.PortStatus = Enum.GetName(typeof(SendPort.SendPortStatusEnum),(int)reader["PortStatus"]);
                            if (reader["LastMessageSentDateTime"] != DBNull.Value)
                                sendPort.LastMessageSentDateTime = (DateTime)reader["LastMessageSentDateTime"];

                            sendPorts.Add(sendPort);
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
                throw new Exception("Exception Occurred in get sendports call. " + ex.Message);
            }
            return sendPorts;
        }
    }
}