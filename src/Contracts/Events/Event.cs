using System.Collections.Generic;

namespace Contracts.Events
{
    public class Event
    {
        public virtual string Type { get; protected set; }

        public string GameId { get; set; }

        public List<string> PlayersIds { get; set; }
    }
}