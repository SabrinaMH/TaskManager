using System;
using System.Collections.Generic;
using EventStore.ClientAPI;
using MediatR;
using TaskManager.Domain.Features.ChangeTaskStatus;
using TaskManager.Domain.Features.ProjectTreeView;
using TaskManager.Domain.Features.RegisterProject;
using TaskManager.Domain.Features.RegisterTask;
using TaskManager.Domain.Features.ReprioritizeProject;
using TaskManager.Domain.Features.TaskGridView;
using TaskManager.Domain.Models.Project;
using TaskManager.Domain.Models.Task;

namespace TaskManager.Domain.Infrastructure
{
    public class Mediate
    {
        private readonly IEventStoreConnectionBuilder _eventStoreConnectionBuilder;
        private IMediator _mediator;

        public Mediate(IEventStoreConnectionBuilder eventStoreConnectionBuilder)
        {
            if (eventStoreConnectionBuilder == null) throw new ArgumentNullException("eventStoreConnectionBuilder");
            _eventStoreConnectionBuilder = eventStoreConnectionBuilder;
            _mediator = new Mediator(SingleInstanceFactory, MultiInstanceFactory);
        }

        public IMediator Mediator
        {
            get { return _mediator; }
        }

        private IEnumerable<object> MultiInstanceFactory(Type serviceType)
        {
            var documentStore = new RavenDbStore().Instance;

            var instances = new List<object>();
            if (serviceType.IsAssignableFrom(typeof (ProjectPriorityQueryHandler)))
            {
                instances.Add(new ProjectPriorityQueryHandler());
            }
            if (serviceType.IsAssignableFrom(typeof(ProjectRegisteredEventHandler)))
            {
                instances.Add(new ProjectRegisteredEventHandler(documentStore));
            }
            if (serviceType.IsAssignableFrom(typeof(TaskRegisteredEventHandler)))
            {
                instances.Add(new TaskRegisteredEventHandler(documentStore));
            }
            if (serviceType.IsAssignableFrom(typeof(TaskDoneEventHandler)))
            {
                instances.Add(new TaskDoneEventHandler(documentStore));
            }
            if (serviceType.IsAssignableFrom(typeof(TaskReopenedEventHandler)))
            {
                instances.Add(new TaskReopenedEventHandler(documentStore));
            }
            return instances;
        }

        private object SingleInstanceFactory(Type serviceType)
        {
            var taskEventStoreRepository = new EventStoreRepository<Task>(_mediator, _eventStoreConnectionBuilder);
            var projectEventStoreRepository = new EventStoreRepository<Project>(_mediator, _eventStoreConnectionBuilder);

            if (serviceType.IsAssignableFrom(typeof (RegisterTaskCommandHandler)))
            {
                return new RegisterTaskCommandHandler(taskEventStoreRepository);
            }
            if (serviceType.IsAssignableFrom(typeof (RegisterProjectCommandHandler)))
            {
                return new RegisterProjectCommandHandler(projectEventStoreRepository);
            }
            if (serviceType.IsAssignableFrom(typeof(ReprioritizeProjectCommandHandler)))
            {
                return new ReprioritizeProjectCommandHandler(projectEventStoreRepository);
            }
            if (serviceType.IsAssignableFrom(typeof(MarkTaskAsDoneCommandHandler)))
            {
                return new MarkTaskAsDoneCommandHandler(taskEventStoreRepository);
            }
            if (serviceType.IsAssignableFrom(typeof(ReopenTaskCommandHandler)))
            {
                return new ReopenTaskCommandHandler(taskEventStoreRepository);
            }
            throw new ArgumentException(serviceType.ToString());
        }
    }
}