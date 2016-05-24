using System;
using System.Runtime.Serialization;

namespace TaskManager.Domain.Features.ReprioritizeTask
{
    [Serializable]
    public class TaskDoesNotExistException : Exception
    {
        public TaskDoesNotExistException()
        {
        }

        public TaskDoesNotExistException(string message) : base(message)
        {
        }

        public TaskDoesNotExistException(string message, Exception inner) : base(message, inner)
        {
        }

        protected TaskDoesNotExistException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}