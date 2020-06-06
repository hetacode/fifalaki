using System.Collections.Generic;
using ProtoContracts;

namespace GamesList.Services
{
    public class ServerState
    {
        public List<(GameItem item, List<string> players)> Games { get; set; } = new List<(GameItem, List<string>)>();
    }
}