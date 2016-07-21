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
using TaskManager.Domain.Models.Project;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Infrastructure
{
    public class CommandDispatcher
    {
        private readonly EventBus _eventBus;
        private readonly Action<Command, Action<Command>> _commandDecorators;
        private readonly IEventStoreConnectionBuilder _eventStoreConnectionBuilder;
        private readonly Dictionary<Type, List<Action<Command>>> _routes =
            new Dictionary<Type, List<Action<Command>>>();

        public CommandDispatcher(IEventStoreConnectionBuilder eventStoreConnectionBuilder, EventBus eventBus)
        {
            if (eventStoreConnectionBuilder == null) throw new ArgumentNullException("eventStoreConnectionBuilder");
            if (eventBus == null) throw new ArgumentNullException("eventBus");
            _eventStoreConnectionBuilder = eventStoreConnectionBuilder;
            _eventBus = eventBus;
            Bootstrap();
        }

        public CommandDispatcher(IEventStoreConnectionBuilder eventStoreConnectionBuilder, EventBus eventBus,
            Action<Command, Action<Command>> commandDecorators)
            : this(eventStoreConnectionBuilder, eventBus)
        {
            _commandDecorators = commandDecorators;
        }

        /// <exception cref="InvalidOperationException"></exception>
        public void Send<T>(T command) where T : Command
        {
            List<Action<Command>> handlers;

            if (_routes.TryGetValue(typeof(T), out handlers))
            {
                if (handlers.Count != 1)
                    throw new InvalidOperationException("Bus cannot send to more than one handler");

                if (_commandDecorators != null)
                {
                    _commandDecorators(command, handlers[0]);
                }
                else
                {
                    handlers[0](command);
                }
            }
            else
            {
                throw new InvalidOperationException("Bus has no handler registered");
            }
        }

        private void Bootstrap()
        {
            var taskEventStoreRepository = new EventStoreRepository<Task>(_eventBus, _eventStoreConnectionBuilder);
            var projectEventStoreRepository = new EventStoreRepository<Project>(_eventBus, _eventStoreConnectionBuilder);

            // Projects
            RegisterHandler<RegisterProject>(
                cmd => new RegisterProjectCommandHandler(projectEventStoreRepository).Handle(cmd));
            RegisterHandler<ReprioritizeProject>(
                cmd => new ReprioritizeProjectCommandHandler(projectEventStoreRepository).Handle(cmd));

            // Tasks
            RegisterHandler<MarkTaskAsDone>(
                cmd => new MarkTaskAsDoneCommandHandler(taskEventStoreRepository).Handle(cmd));
            RegisterHandler<ReopenTask>(
                cmd => new ReopenTaskCommandHandler(taskEventStoreRepository).Handle(cmd));
            RegisterHandler<ReprioritizeTask>(
                cmd => new ReprioritizeTaskCommandHandler(taskEventStoreRepository).Handle(cmd));
            RegisterHandler<ChangeDeadlineOnTask>(
                 cmd => new ChangeDeadlineOnTaskCommandHandler(taskEventStoreRepository).Handle(cmd));
            RegisterHandler<RegisterTask>(
                cmd => new RegisterTaskCommandHandler(taskEventStoreRepository).Handle(cmd));
            RegisterHandler<ChangeTitleOnTask>(
                cmd => new ChangeTitleOnTaskCommandHandler(taskEventStoreRepository).Handle(cmd));

            // Notes
            RegisterHandler<SaveNote>(
                cmd => new SaveNoteCommandHandler(taskEventStoreRepository).Handle(cmd));
            RegisterHandler<EraseNote>(
                cmd => new EraseNoteCommandHandler(taskEventStoreRepository).Handle(cmd));
        }

        private void RegisterHandler<T>(Action<T> handler) where T : Command
        {
            List<Action<Command>> handlers;

            if (!_routes.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<Action<Command>>();
                _routes.Add(typeof(T), handlers);
            }

            handlers.Add((x => handler((T)x)));
        }
    }
}