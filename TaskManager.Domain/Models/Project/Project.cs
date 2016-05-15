using System;
using System.Collections.Generic;
using TaskManager.Domain.Common;
using TaskManager.Domain.Features.RegisterProject;
using TaskManager.Domain.Features.ReprioritizeProject;
using TaskManager.Domain.Models.Common;

namespace TaskManager.Domain.Models.Project
{
    public class Project : AggregateRoot
    {
        private Deadline _deadline;
        private Title _title;
        private ProjectPriority _priority;

        public Project(IList<Event> history) : base(history) { }

        public Project(Title title) : base(ProjectId.Create(title))
        {
            if (title == null) throw new ArgumentNullException("title");
            ApplyChange(new ProjectRegistered(Id, title, ProjectPriority.None.DisplayName));
        }

        public Project(Title title, Deadline deadline) : base(ProjectId.Create(title))
        {
            if (title == null) throw new ArgumentNullException("title");
            if (deadline == null) throw new ArgumentNullException("deadline");
            ApplyChange(new ProjectRegistered(Id, title, ProjectPriority.None.DisplayName, deadline));
        }

        public void Reprioritize(ProjectPriority newPriority)
        {
            if (newPriority == null) throw new ArgumentNullException("newPriority");
            if (newPriority.Equals(_priority)) return;

            ApplyChange(new ProjectReprioritized(Id, _priority.DisplayName, newPriority.DisplayName));
        }

        private void Apply(ProjectRegistered @event)
        {
            Id = new ProjectId(@event.ProjectId);
            _title = new Title(@event.Title);
            _priority = ProjectPriority.Parse(@event.Priority);
            if (!string.IsNullOrWhiteSpace(@event.Deadline))
            {
                _deadline = new Deadline(DateTime.Parse(@event.Deadline));
            }
        }

        private void Apply(ProjectReprioritized @event)
        {
            Id = new ProjectId(@event.ProjectId);
            ProjectPriority.TryParse(@event.NewPriority, out _priority);
        }
    }
}