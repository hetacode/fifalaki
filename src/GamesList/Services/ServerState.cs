using System.Collections.Generic;
using ProtoContracts;

namespace GamesList.Services
{
    public class ServerState
    {
        public List<GameItem> Games { get; set; } = new List<GameItem>();
    }
}