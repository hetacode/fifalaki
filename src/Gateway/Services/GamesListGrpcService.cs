using Microsoft.Extensions.Configuration;
using Grpc.Net.Client;
using ProtoContracts;
using System.Threading.Tasks;
using Contracts.Response;
using System.Linq;
using System;

namespace Gateway.Services
{
    public class GamesListGrpcService
    {
        private readonly IConfiguration _config;
        private GamesListService.GamesListServiceClient _client;

        public GamesListGrpcService(IConfiguration config)
            => _config = config;

        public void Init()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var channel = GrpcChannel.ForAddress(_config["GamesListSvc_Endpoint"]);
            _client = new GamesListService.GamesListServiceClient(channel);
        }

        public async Task<GetGamesListResponse> GetGamesList()
        {
            // using var channel = GrpcChannel.ForAddress(_config["GamesListSvc_Endpoint"]);
            // _client = new GamesListService.GamesListServiceClient(channel);
            var res = await _client.GetGamesListAsync(new GetGamesListReq());

            var output = new GetGamesListResponse
            {
                Items = res.Items
                           .Select(s => new Contracts.Response.GameItem { Id = s.Id, PlayersCount = s.PlayersCount })
                           .ToList()
            };

            return output;
        }
    }
}