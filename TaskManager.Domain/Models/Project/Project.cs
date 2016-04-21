using System;
using System.Collections.Generic;
using TaskManager.Domain.Common;
using TaskManager.Domain.Features.RegisterProject;

namespace TaskManager.Domain.Models.Project
{
    public class Project : AggregateRoot
    {
        private Deadline _deadline;
        private Title _title;

        public Project(IList<Event> history) : base(history) { }

        public Project(Title title) : base(new ProjectId(Guid.NewGuid()))
        {
            if (title == null) throw new ArgumentNullException("title");
            ApplyChange(new ProjectRegistered(Id, title));
        }

        public Project(Title title, Deadline deadline) : base(new ProjectId(Guid.NewGuid()))
        {
            if (title == null) throw new ArgumentNullException("title");
            if (deadline == null) throw new ArgumentNullException("deadline");
            _title = title;
            _deadline = deadline;
            ApplyChange(new ProjectRegistered(Id, title, deadline));
        }

        public void Prioritize(Priority priority)
        {
            if (priority == null) throw new ArgumentNullException("priority");
            ApplyChange(new ProjectPrioritized(Id, priority.DisplayName));
        }

        private void Apply(ProjectRegistered @event)
        {
            Id = new ProjectId(@event.ProjectId);
            _title = new Title(@event.Title);
            if (!string.IsNullOrWhiteSpace(@event.Deadline))
            {
                _deadline = new Deadline(DateTime.Parse(@event.Deadline));
            }
        }

        private void Apply(ProjectPrioritized @event)
        {
            Id = new ProjectId(@event.ProjectId);
            Priority priority;
            Priority.TryParse(@event.Priority, out priority);
        }
    }
}