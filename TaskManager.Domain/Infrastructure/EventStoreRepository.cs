using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.ClientAPI.Exceptions;
using EventStore.Core;
using EventStore.Core.Data;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TaskManager.Domain.Common;
using Event = TaskManager.Domain.Common.Event;
using ExpectedVersion = EventStore.ClientAPI.ExpectedVersion;
using ResolvedEvent = EventStore.ClientAPI.ResolvedEvent;

namespace TaskManager.Domain.Infrastructure
{
    public class EventStoreRepository<TAggregate> where TAggregate : AggregateRoot
    {
        private readonly IMediator _mediator;
        private const string EventClrTypeHeader = "EventClrTypeName";
        private const string AggregateClrTypeHeader = "AggregateClrTypeName";
        private const string CommitIdHeader = "CommitId";
        private const int ReadPageSize = 200;

        private readonly IEventStoreConnection _connection;

        public EventStoreRepository(IMediator mediator, IEventStoreConnection eventStoreConnection)
        {
            if (mediator == null) throw new ArgumentNullException("mediator");
            if (eventStoreConnection == null) throw new ArgumentNullException("eventStoreConnection");
            _mediator = mediator;
            _connection = eventStoreConnection;
            _connection.ConnectAsync().Wait();
        }

        public EventStoreRepository()
        {
            var mediate = new Mediate();
            _mediator = mediate.Bootstrap();

            _connection = UseInMemoryEventStore();  //EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            _connection.ConnectAsync().Wait();
        }

        private IEventStoreConnection UseInMemoryEventStore()
        {
            ClusterVNode node = EmbeddedVNodeBuilder.AsSingleNode().RunInMemory().OnDefaultEndpoints().Build();

            bool isNodeMaster = false;
            node.NodeStatusChanged += (sender, args) => isNodeMaster = args.NewVNodeState == VNodeState.Master;
            node.Start();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (!isNodeMaster)
            {
                if (stopwatch.Elapsed.Seconds > 20)
                {
                    throw new EventStoreConnectionException("In memory node failed to become master within the time limit");
                }
                Thread.Sleep(1);
            }
            stopwatch.Stop();

            IEventStoreConnection eventStoreConnection = EmbeddedEventStoreConnection.Create(node);
            return eventStoreConnection;
        }

        public TAggregate GetById(string id) 
        {
            var events = new List<Event>();
            StreamEventsSlice currentSlice;
            var nextSliceStart = StreamPosition.Start;
            var streamName = GetStreamName(typeof(TAggregate), id);

            do
            {
                currentSlice = _connection
                    .ReadStreamEventsForwardAsync(streamName, nextSliceStart, ReadPageSize, false)
                    .Result;
                nextSliceStart = currentSlice.NextEventNumber;
                events.AddRange(currentSlice.Events.Select(x => DeserializeEvent(x)));
            } while (!currentSlice.IsEndOfStream);
            if (!events.Any())
            {
                return null;
            }

            var constructor = typeof(TAggregate).GetConstructor(new Type[] { typeof(IList<Event>) });
            var aggregate = (TAggregate) constructor.Invoke(new object[] { events });
            return aggregate;
        }

        private string GetStreamName(Type type, string id)
        {
            return string.Format("{0}-{1}", type.Name, id);
        }

        private Event DeserializeEvent(ResolvedEvent e)
        {
            var eventClrTypeName =
                JObject.Parse(Encoding.UTF8.GetString(e.OriginalEvent.Metadata)).Property(EventClrTypeHeader).Value;
            string data = Encoding.UTF8.GetString(e.OriginalEvent.Data);
            Type type = Type.GetType((string) eventClrTypeName);
            var obj = JsonConvert.DeserializeObject(data, type);
            return (Event) obj;
        }

        public void Save(AggregateRoot aggregate)
        {
            string streamName = GetStreamName(aggregate.GetType(), aggregate.Id);
            List<Event> newEvents = aggregate.GetUncommittedEvents().ToList();
            int originalVersion = aggregate.Version - newEvents.Count;
            int expectedVersion = originalVersion == 0 ? ExpectedVersion.NoStream : originalVersion - 1;
            
            var commitId = Guid.NewGuid();
            var commitHeaders = new Dictionary<string, object>
            {
                {CommitIdHeader, commitId},
                {AggregateClrTypeHeader, aggregate.GetType().AssemblyQualifiedName}
            };
            
            List<EventData> eventsToSave = newEvents.Select(e => ToEventData(Guid.NewGuid(), e, commitHeaders)).ToList();
            _connection.AppendToStreamAsync(streamName, expectedVersion, eventsToSave).Wait();

            foreach (var eventToPublish in newEvents)
            {
                _mediator.Publish(eventToPublish);
            }

            aggregate.MarkChangesAsCommitted();
        }

        private static EventData ToEventData(Guid eventId, object evnt, IDictionary<string, object> headers)
        {
            var serializerSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.None};
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evnt, serializerSettings));

            var eventHeaders = new Dictionary<string, object>(headers)
            {
                {
                    EventClrTypeHeader, evnt.GetType().AssemblyQualifiedName
                }
            };
            byte[] metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, serializerSettings));
            string typeName = evnt.GetType().Name;

            return new EventData(eventId, typeName, true, data, metadata);
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}