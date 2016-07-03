using System;
using System.Collections.Generic;
using TaskManager.Domain.Common;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Models.Project;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Infrastructure
{
    public class EventBus
    {
        private readonly Action<Event, Action<Event>> _eventDecorators;
        private readonly Dictionary<Type, List<Action<Event>>> _routes =
            new Dictionary<Type, List<Action<Event>>>();

        public EventBus(Action<Event, Action<Event>> eventDecorators)
        {
            _eventDecorators = eventDecorators;
            Bootstrap();
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
            RegisterHandler<ProjectRegistered>(
                @event => new ProjectRegisteredEventHandler(documentStore).Handle(@event));
            RegisterHandler<TaskRegistered>(
                @event => new TaskRegisteredEventHandler(documentStore).Handle(@event));
            RegisterHandler<TaskDone>(
                @event => new TaskDoneEventHandler(documentStore).Handle(@event));
            RegisterHandler<TaskReopened>(
                @event => new TaskReopenedEventHandler(documentStore).Handle(@event));
            RegisterHandler<TaskReprioritized>(
                @event => new TaskReprioritizedEventHandler(documentStore).Handle(@event));
            RegisterHandler<ProjectReprioritized>(
                @event => new ProjectReprioritizedEventHandler(documentStore).Handle(@event));
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