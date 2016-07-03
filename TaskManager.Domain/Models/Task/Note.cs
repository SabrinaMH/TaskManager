using System;

namespace TaskManager.Domain.Models.Task
{
    public class Note
    {
        public string Content { get; private set; }

        /// <exception cref="ArgumentException">content cannot be null or empty</exception>
        public Note(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("content cannot be null or empty", "content");
            Content = content;
        }
    }
}