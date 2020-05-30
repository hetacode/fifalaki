using System.Threading.Tasks;
using Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController
    {
        private readonly GamesListGrpcService _gamesListService;

        public GamesController(GamesListGrpcService gamesListService)
            => _gamesListService = gamesListService;

        [HttpGet("list")]
        public async Task<JsonResult> GetGamesList()
        {
            var result = await _gamesListService.GetGamesList();
            return new JsonResult(result);
        }
    }
}