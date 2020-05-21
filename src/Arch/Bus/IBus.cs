using System;
using System.Threading.Tasks;
using Contracts.Events;

namespace Arch.Bus
{
    public interface IBus
    {
        void Consumer(Action<(string eventType, string body)> callback);

        Task Publish(Event Event);
    }
}