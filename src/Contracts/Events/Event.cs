using System.Collections.Generic;

namespace Contracts.Events
{
    public class Event
    {
        public virtual string Type { get; protected set; }
    }
}