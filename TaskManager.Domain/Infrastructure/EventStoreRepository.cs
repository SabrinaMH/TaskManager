﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using EventStore.ClientAPI;
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
        private readonly IEventStoreConnectionBuilder _eventStoreConnectionBuilder;
        private const string EventClrTypeHeader = "EventClrTypeName";
        private const string AggregateClrTypeHeader = "AggregateClrTypeName";
        private const string CommitIdHeader = "CommitId";
        private const int ReadPageSize = 200;

        public EventStoreRepository(IMediator mediator, IEventStoreConnectionBuilder eventStoreConnectionBuilder)
        {
            if (mediator == null) throw new ArgumentNullException("mediator");
            if (eventStoreConnectionBuilder == null) throw new ArgumentNullException("eventStoreConnectionBuilder");
            _eventStoreConnectionBuilder = eventStoreConnectionBuilder;
            _mediator = mediator;
        }
        
        public TAggregate GetById(string id)
        {
            using (var connection = _eventStoreConnectionBuilder.Build())
            {
                connection.ConnectAsync().Wait();
                var events = new List<Event>();
                StreamEventsSlice currentSlice;
                var nextSliceStart = StreamPosition.Start;
                var streamName = GetStreamName(typeof (TAggregate), id);

                do
                {
                    currentSlice = connection
                        .ReadStreamEventsForwardAsync(streamName, nextSliceStart, ReadPageSize, false)
                        .Result;
                    nextSliceStart = currentSlice.NextEventNumber;
                    events.AddRange(currentSlice.Events.Select(x => DeserializeEvent(x)));
                } while (!currentSlice.IsEndOfStream);
                if (!events.Any())
                {
                    return null;
                }

                var constructor = typeof (TAggregate).GetConstructor(new Type[] {typeof (IList<Event>)});
                var aggregate = (TAggregate) constructor.Invoke(new object[] {events});
                return aggregate;
            }
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
            List<Event> newEvents = aggregate.GetUncommittedEvents().ToList();
            using (var connection = _eventStoreConnectionBuilder.Build())
            {
                connection.ConnectAsync().Wait();
                string streamName = GetStreamName(aggregate.GetType(), aggregate.Id);
                int originalVersion = aggregate.Version - newEvents.Count;
                int expectedVersion = originalVersion == 0 ? ExpectedVersion.NoStream : originalVersion - 1;

                var commitId = Guid.NewGuid();
                var commitHeaders = new Dictionary<string, object>
                {
                    {CommitIdHeader, commitId},
                    {AggregateClrTypeHeader, aggregate.GetType().AssemblyQualifiedName}
                };

                List<EventData> eventsToSave =
                    newEvents.Select(e => ToEventData(Guid.NewGuid(), e, commitHeaders)).ToList();
                connection.AppendToStreamAsync(streamName, expectedVersion, eventsToSave).Wait();
            }
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
    }
}