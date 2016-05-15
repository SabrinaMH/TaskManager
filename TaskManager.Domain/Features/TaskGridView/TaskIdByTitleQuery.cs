using System;

namespace TaskManager.Domain.Features.TaskGridView
{
    public class TaskIdByTitleQuery
    {
        public string Title { get; private set; }

        /// <exception cref="ArgumentException">title cannot be null or empty</exception>
        public TaskIdByTitleQuery(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("title cannot be null or empty", "title");
            Title = title;
        }
    }
}