using System.Management;
using BiztalkAdminAPI.Models;
using Microsoft.AspNetCore.Mvc;
using BiztalkAdminAPI.Services;

namespace BiztalkAdminAPI.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class BizTalkStatusController : ControllerBase
    {
        private readonly IBizTalkStatusService _service;

        public BizTalkStatusController(IBizTalkStatusService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// Gets overall status of BizTalk Server
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public BizTalkServerStatus Get()
        {
            var BtsStatus = _service.GetBizTalkServerStatus();
            
            return BtsStatus;
        }
    }
}