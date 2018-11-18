using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using server.Hubs;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        //logger
        private readonly ILogger<AccountController> _logger;

        //signalr hubs
        private readonly IHubContext<ChatHub> _chatHub;

        public AccountController(ILogger<AccountController> logger,
            IHubContext<ChatHub> chatHub)
        {
            _logger = logger;
            _chatHub = chatHub;
        }

        [HttpPost]
        public async Task<IActionResult> Register()
        {
            return await Task.Run(() => throw new NotImplementedException);
        }
    }
}