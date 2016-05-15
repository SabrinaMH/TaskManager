using EventStore.ClientAPI;

namespace TaskManager.Domain.Infrastructure
{
    public interface IEventStoreConnectionBuilder
    {
        IEventStoreConnection Build();
    }
}