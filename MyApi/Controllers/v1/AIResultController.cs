using Asp.Versioning;
using Kolbeh.Api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.Hubs;
using Services.Services.NotifHubs;
using WebFramework.Api;

namespace Kolbeh.Api.Controllers.v1
{
  /// <summary>
  /// نتیجه ی AI
  /// </summary>
    [ApiVersion("1")]
    [ApiController]
    public class AIResultController : BaseController
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IConnectionMappingService _connectionService;

        public AIResultController(IHubContext<NotificationHub> hubContext, IConnectionMappingService connectionService)
        {
            _hubContext = hubContext;
            _connectionService = connectionService;
        }



        [HttpPost("[action]")]
        public async Task<IActionResult> PostResult([FromBody] AIResultDto result)
        {
            var connections = _connectionService.GetConnections(result.UserId);
            foreach (var connectionId in connections)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveAIResult", result.Message);
            }
            return Ok(new { success = true });
        }
    }

}
