using System;
using System.Collections.Generic;
using TaskManager.Domain.Common;
using TaskManager.Domain.Features.ChangeDeadlineOnTask;
using TaskManager.Domain.Features.ChangeTaskStatus;
using TaskManager.Domain.Features.ChangeTitleOnTask;
using TaskManager.Domain.Features.EraseNote;
using TaskManager.Domain.Features.RegisterProject;
using TaskManager.Domain.Features.RegisterTask;
using TaskManager.Domain.Features.ReprioritizeProject;
using TaskManager.Domain.Features.ReprioritizeTask;
using TaskManager.Domain.Features.SaveNote;
using TaskManager.Domain.Features.TaskGridView;
using ProjectTreeView = TaskManager.Domain.Features.ProjectTreeView;

namespace TaskManager.Domain.Infrastructure
{
    public class EventBus
    {
        private readonly Action<Event, Action<Event>> _eventDecorators;
        private readonly Dictionary<Type, List<Action<Event>>> _routes =
            new Dictionary<Type, List<Action<Event>>>();

        public EventBus()
        {
            Bootstrap();
        }

        public EventBus(Action<Event, Action<Event>> eventDecorators)
            : this()
        {
            _eventDecorators = eventDecorators;
        }
        
        public void Publish<T>(T @event) where T : Event
        {
            List<Action<Event>> handlers;
            
            if (!_routes.TryGetValue(@event.GetType(), out handlers))
            {
                return;
            }

            foreach (var handler in handlers)
            {
                if (_eventDecorators != null)
                {
                    _eventDecorators(@event, handler);
                }
                else
                {
                    handler(@event);
                }
            }
        }

        private void Bootstrap()
        {
            var documentStore = new RavenDbStore().Instance;

            // Project
            RegisterHandler<ProjectRegistered>(
                @event => new ProjectTreeView.ProjectRegisteredEventHandler(documentStore).Handle(@event));
            RegisterHandler<ProjectReprioritized>(
                @event => new ProjectTreeView.ProjectReprioritizedEventHandler(documentStore).Handle(@event));
            RegisterHandler<TaskDone>(
                @event => new ProjectTreeView.TaskDoneEventHandler(documentStore).Handle(@event));
            RegisterHandler<TaskReopened>(
                            @event => new ProjectTreeView.TaskReopenedEventHandler(documentStore).Handle(@event));
            RegisterHandler<TaskRegistered>(
                            @event => new ProjectTreeView.TaskRegisteredEventHandler(documentStore).Handle(@event));

            // Task
            RegisterHandler<TaskRegistered>(
                @event => new TaskRegisteredEventHandler(documentStore).Handle(@event));
            RegisterHandler<TaskDone>(
                @event => new TaskDoneEventHandler(documentStore).Handle(@event));
            RegisterHandler<TaskReopened>(
                @event => new TaskReopenedEventHandler(documentStore).Handle(@event));
            RegisterHandler<TaskReprioritized>(
                @event => new TaskReprioritizedEventHandler(documentStore).Handle(@event));
            RegisterHandler<TitleOnTaskChanged>(
                @event => new TitleOnTaskChangedEventHandler(documentStore).Handle(@event));
            RegisterHandler<DeadlineOnTaskChanged>(
                 @event => new DeadlineOnTaskChangedEventHandler(documentStore).Handle(@event));
            
            // Note
            RegisterHandler<NoteSaved>(
                @event => new NoteSavedEventHandler(documentStore).Handle(@event));
            RegisterHandler<NoteErased>(
                @event => new NoteErasedEventHandler(documentStore).Handle(@event));
        }

        private void RegisterHandler<T>(Action<T> handler) where T : Event
        {
            List<Action<Event>> handlers;

            if (!_routes.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<Action<Event>>();
                _routes.Add(typeof(T), handlers);
            }

            handlers.Add((x => handler((T)x)));
        }
    }
}