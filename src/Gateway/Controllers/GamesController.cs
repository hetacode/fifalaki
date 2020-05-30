using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController
    {
        [HttpGet("games")]
        public JsonResult GetGamesList()
        {
            return new JsonResult(new { Beka = "ff" });
        }
    }
}