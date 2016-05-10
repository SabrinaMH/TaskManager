using System;
using System.Runtime.Serialization;

namespace TaskManager.Domain.Features.RegisterTask
{
    [Serializable]
    public class TaskWithSameTitleExistsInProjectException : Exception
    {
        public TaskWithSameTitleExistsInProjectException()
        {
        }

        public TaskWithSameTitleExistsInProjectException(string message) : base(message)
        {
        }

        public TaskWithSameTitleExistsInProjectException(string message, Exception inner) : base(message, inner)
        {
        }

        protected TaskWithSameTitleExistsInProjectException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}