using TaskManager.Domain.Common;

namespace TaskManager.Domain.Features.EraseNote
{
    public class NoteErased : Event
    {
        public string TaskId { get; private set; }

        public NoteErased(string taskId)
        {
            TaskId = taskId;
        }
    }
}