using System;

namespace TaskManager.Domain.Features.RegisterTask
{
    public class DoesTaskWithTitleAlreadyExistUnderSameProjectQuery
    {
        public string Title { get; private set; }

        public DoesTaskWithTitleAlreadyExistUnderSameProjectQuery(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("title cannot be null or empty", "title");
            Title = title;
        }
    }
}