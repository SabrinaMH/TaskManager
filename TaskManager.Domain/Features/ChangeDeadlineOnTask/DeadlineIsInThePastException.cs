using System;
using System.Runtime.Serialization;

namespace TaskManager.Domain.Features.ChangeDeadlineOnTask
{
    [Serializable]
    public class DeadlineIsInThePastException : Exception
    {
        public DeadlineIsInThePastException()
        {
        }

        public DeadlineIsInThePastException(string message) : base(message)
        {
        }

        public DeadlineIsInThePastException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DeadlineIsInThePastException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}