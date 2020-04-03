using System;
using Xunit;
using BiztalkAdminAPI.Controllers;
using BiztalkAdminAPI.Services;
using BiztalkAdminAPI.Models;

namespace BiztalkAdminAPI.Tests
{
    public class BizTalkStatusControllerTest
    {
        BizTalkStatusController _controller;
        IBizTalkStatusService _service;

        public BizTalkStatusControllerTest()
        {
            _service = new BizTalkStatusServiceFake();
            _controller = new BizTalkStatusController(_service);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsStatus()
        {
            // Act
            var status = _controller.Get() as BizTalkServerStatus;

            // Assert
            Assert.Equal(2, status.HostInstancesRunning);
        }
    }
}
