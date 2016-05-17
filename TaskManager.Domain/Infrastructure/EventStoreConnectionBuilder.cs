using System.Net;
using EventStore.ClientAPI;

namespace TaskManager.Domain.Infrastructure
{
    public class EventStoreConnectionBuilder : IEventStoreConnectionBuilder
    {
        public IEventStoreConnection Build()
        {
            ConnectionSettings connectionSettings = ConnectionSettings.Create().Build();
            return EventStoreConnection.Create(connectionSettings, new IPEndPoint(IPAddress.Loopback, 1113));
        }
    }
}