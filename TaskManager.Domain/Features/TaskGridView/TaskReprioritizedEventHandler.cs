﻿using Raven.Client;
using TaskManager.Domain.Features.ReprioritizeTask;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Features.TaskGridView
{
    public class TaskReprioritizedEventHandler 
    {
        private readonly IDocumentStore _documentStore;

        public TaskReprioritizedEventHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(TaskReprioritized @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var taskInGridView = session.Load<TaskInGridView>(@event.TaskId);
                taskInGridView.Priority = @event.NewPriority;
                session.SaveChanges();
            }
        }
    }
}