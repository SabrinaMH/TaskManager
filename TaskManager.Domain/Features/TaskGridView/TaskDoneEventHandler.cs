﻿using Raven.Client;
using TaskManager.Domain.Features.ChangeTaskStatus;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.TaskGridView
{
    public class TaskDoneEventHandler
    {
        private readonly IDocumentStore _documentStore;

        public TaskDoneEventHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(TaskDone @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var taskInGridView = session.Load<TaskInGridView>(@event.TaskId);
                taskInGridView.IsDone = true;
                session.SaveChanges();
            }
        }
    }
}