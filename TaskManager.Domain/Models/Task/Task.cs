using System;
using System.Collections.Generic;
using TaskManager.Domain.Common;
using TaskManager.Domain.Features.ChangeDeadlineOnTask;
using TaskManager.Domain.Features.ChangeTaskStatus;
using TaskManager.Domain.Features.ChangeTitleOnTask;
using TaskManager.Domain.Features.EraseNote;
using TaskManager.Domain.Features.RegisterTask;
using TaskManager.Domain.Features.ReprioritizeTask;
using TaskManager.Domain.Features.SaveNote;
using TaskManager.Domain.Models.Common;
using TaskManager.Domain.Models.Project;

namespace TaskManager.Domain.Models.Task
{
    public class Task : AggregateRoot
    {
        private TaskDeadline _deadline;
        private Title _title;
        private TaskPriority _priority;
        private ProjectId _projectId;
        private bool _isDone;
        private Note _note;

        public Task(IList<Event> history) : base(history)
        {
        }

        public Task(ProjectId projectId, Title title, TaskPriority priority) : base(TaskId.Create(projectId, title))
        {
            if (title == null) throw new ArgumentNullException("title");
            if (priority == null) throw new ArgumentNullException("priority");
            ApplyChange(new TaskRegistered(Id, projectId, title, priority.DisplayName));
        }

        public Task(ProjectId projectId, Title title, TaskPriority priority, TaskDeadline deadline)
            : base(TaskId.Create(projectId, title))
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

        /// <exception cref="ArgumentNullException"><paramref name="note"/> is <see langword="null" />.</exception>
        public void SaveNote(Note note)
        {
            if (note == null) throw new ArgumentNullException("note");
            ApplyChange(new NoteSaved(Id, note.Content));
        }

        public void EraseNote()
        {
            if (_note == null) return;
            ApplyChange(new NoteErased(Id));
        }

        /// <exception cref="DeadlineIsInThePastException">Condition.</exception>
        public void ChangeDeadline(TaskDeadline deadline)
        {
            if (deadline.InThePast) throw new DeadlineIsInThePastException();
            ApplyChange(new DeadlineOnTaskChanged(Id, deadline));
        }

        public void Done()
        {
            if (!_isDone)
                ApplyChange(new TaskDone(Id, _projectId));
        }

        public void Reopen()
        {
            if (_isDone)
                ApplyChange(new TaskReopened(Id, _projectId));
        }

        /// <exception cref="ArgumentNullException"><paramref name="newTitle"/> is <see langword="null" />.</exception>
        public void ChangeTitle(Title newTitle)
        {
            if (newTitle == null) throw new ArgumentNullException("newTitle");
            ApplyChange(new TitleOnTaskChanged(Id, newTitle));
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
                _deadline = new TaskDeadline(DateTime.Parse(@event.Deadline));
            }
        }

        private void Apply(TaskReprioritized @event)
        {
            Id = new TaskId(@event.TaskId);
            _priority = TaskPriority.Parse(@event.NewPriority);
        }

        private void Apply(NoteSaved @event)
        {
            _note = new Note(@event.NoteContent);
        }

        private void Apply(NoteErased @event)
        {
            _note = null;
        }

        private void Apply(TitleOnTaskChanged @event)
        {
            _title = new Title(@event.NewTitle);
        }

        private void Apply(DeadlineOnTaskChanged @event)
        {
            _deadline = new TaskDeadline(DateTime.Parse(@event.NewDeadline));
        }
    }
}