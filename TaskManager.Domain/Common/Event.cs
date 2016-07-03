
namespace TaskManager.Domain.Common
{
    public abstract class Event 
    {
        public int Version { get; set; }
    }
}