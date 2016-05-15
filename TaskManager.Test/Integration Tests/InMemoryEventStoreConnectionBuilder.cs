using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.Core;
using TaskManager.Domain.Infrastructure;

namespace TaskManager.Test
{
    public class InMemoryEventStoreConnectionBuilder : IEventStoreConnectionBuilder
    {
        public IEventStoreConnection Build()
        {
            ClusterVNode node = new InMemoryEventStore().Instance;
            IEventStoreConnection eventStoreConnection = EmbeddedEventStoreConnection.Create(node);
            return eventStoreConnection;
        }
    }
}