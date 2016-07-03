using System;
using System.Runtime.Serialization;

namespace TaskManager.ProjectTreeViewUI
{
    [Serializable]
    public class SortFailedException : Exception
    {
        public SortFailedException()
        {
        }

        public SortFailedException(string message) : base(message)
        {
        }

        public SortFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SortFailedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}