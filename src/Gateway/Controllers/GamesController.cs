using System.Text.Json;
using System.Threading.Tasks;
using Arch;
using Contracts.Events;
using Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController
    {
        private readonly GamesListGrpcService _gamesListService;
        private readonly GamesBusService _gamesBus;

        public GamesController(GamesListGrpcService gamesListService, GamesBusService gamesBus)
            => (_gamesListService, _gamesBus) = (gamesListService, gamesBus);

        [HttpGet("list")]
        public async Task<JsonResult> GetGamesList()
        {
            var result = await _gamesListService.GetGamesList();
            return new JsonResult(result);
        }

        [HttpPost("event")]
        public async Task<JsonResult> Event([FromBody] JsonElement json)
        {
            var eventType = EventRecognizer.GetEventBy(json.GetProperty("Type").GetString());
            var s = JsonSerializer.Serialize(json);
            var e = JsonSerializer.Deserialize(s, eventType);
            await _gamesBus.SendEvent(e);
            return new JsonResult(new { IsSuccess = true });
        }
    }
}