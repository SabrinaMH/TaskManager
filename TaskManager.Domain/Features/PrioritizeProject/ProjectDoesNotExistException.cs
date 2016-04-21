using System;
using System.Runtime.Serialization;

namespace TaskManager.Domain.Features.PrioritizeProject
{
    [Serializable]
    public class ProjectDoesNotExistException : Exception
    {
        public ProjectDoesNotExistException()
        {
        }

        public ProjectDoesNotExistException(string message) : base(message)
        {
        }

        public ProjectDoesNotExistException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ProjectDoesNotExistException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}