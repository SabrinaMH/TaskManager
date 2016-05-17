using System;
using System.Collections.Generic;
using TaskManager.Domain.Common;
using TaskManager.Domain.Features.ChangeTaskStatus;
using TaskManager.Domain.Features.RegisterTask;
using TaskManager.Domain.Features.ReprioritizeTask;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Domain.Models.Task
{
    public class Task : AggregateRoot
    {
        private Deadline _deadline;
        private Title _title;
        private TaskPriority _priority;
        private ProjectId _projectId;
        private bool _isDone;

        public Task(IList<Event> history) : base(history)
        {
        }

        public Task(ProjectId projectId, Title title, TaskPriority priority) : base(TaskId.Create(title))
        {
            if (title == null) throw new ArgumentNullException("title");
            if (priority == null) throw new ArgumentNullException("priority");
            ApplyChange(new TaskRegistered(Id, projectId, title, priority.DisplayName));
        }

        public Task(ProjectId projectId, Title title, TaskPriority priority, Deadline deadline)
            : base(TaskId.Create(title))
        {
            if (title == null) throw new ArgumentNullException("title");
            if (priority == null) throw new ArgumentNullException("priority");
            if (deadline == null) throw new ArgumentNullException("deadline");
            ApplyChange(new TaskRegistered(Id, projectId, title, priority.DisplayName, deadline));
        }

        public void Reprioritize(TaskPriority newPriority)
        {
            if (newPriority == null) throw new ArgumentNullException("newPriority");
            if (newPriority.Equals(_priority)) return;

            ApplyChange(new TaskReprioritized(Id, _priority.DisplayName, newPriority.DisplayName));
        }

        public void Done()
        {
            if (!_isDone)
                ApplyChange(new TaskDone(Id));
        }

        public void Reopen()
        {
            if (_isDone)
                ApplyChange(new TaskReopened(Id));
        }

        private void Apply(TaskReopened @event)
        {
            _isDone = false;
        }

        private void Apply(TaskDone @event)
        {
            _isDone = true;
        }

        private void Apply(TaskRegistered @event)
        {
            Id = new TaskId(@event.TaskId);
            _projectId = new ProjectId(@event.ProjectId);
            _title = new Title(@event.Title);
            _priority = TaskPriority.Parse(@event.Priority);
            if (!string.IsNullOrWhiteSpace(@event.Deadline))
            {
                _deadline = new Deadline(DateTime.Parse(@event.Deadline));
            }
        }

        private void Apply(TaskReprioritized @event)
        {
            Id = new TaskId(@event.TaskId);
            TaskPriority.TryParse(@event.NewPriority, out _priority);
        }
    }
}