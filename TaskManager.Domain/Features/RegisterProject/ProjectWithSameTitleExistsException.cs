using System;
using System.Runtime.Serialization;

namespace TaskManager.Domain.Features.RegisterProject
{
    [Serializable]
    public class ProjectWithSameTitleExistsException : Exception
    {
        public ProjectWithSameTitleExistsException()
        {
        }

        public ProjectWithSameTitleExistsException(string message) : base(message)
        {
        }

        public ProjectWithSameTitleExistsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ProjectWithSameTitleExistsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}