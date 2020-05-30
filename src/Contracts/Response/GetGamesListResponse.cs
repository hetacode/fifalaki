using System.Collections.Generic;

namespace Contracts.Response
{
    public class GetGamesListResponse
    {
        public List<GameItem> Items { get; set; }
    }

    public class GameItem
    {
        public string Id { get; set; }

        public int PlayersCount { get; set; }
    }
}