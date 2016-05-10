using System;
using System.Runtime.Serialization;

namespace TaskManager.Domain.Models.Common
{
    [Serializable]
    public class UnknownPriorityException : Exception
    {
        public UnknownPriorityException()
        {
        }

        public UnknownPriorityException(string message) : base(message)
        {
        }

        public UnknownPriorityException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UnknownPriorityException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}