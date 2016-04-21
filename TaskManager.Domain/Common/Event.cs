using MediatR;

namespace TaskManager.Domain.Common
{
    public abstract class Event : INotification
    {
        public int Version { get; set; }
    }
}