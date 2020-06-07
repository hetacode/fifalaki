using System.Linq;
using System.Threading.Tasks;
using ProtoContracts;

namespace GamesList.Services
{
    public class GamesListService : ProtoContracts.GamesListService.GamesListServiceBase
    {
        private readonly ServerState _state;

        public GamesListService(ServerState state)
            => _state = state;
        public override async Task<ProtoContracts.GetGamesListRes> GetGamesList(ProtoContracts.GetGamesListReq request, Grpc.Core.ServerCallContext context)
        {
            var response = new GetGamesListRes();
            response.Items.AddRange(_state.Games.Select(s => s.item));
            return response;
        }
    }
}