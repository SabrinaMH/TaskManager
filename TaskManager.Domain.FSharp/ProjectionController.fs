namespace TaskManager.Domain.FSharp.Controllers

open System.Web.Http
open Raven.Client;
open EventStore.ClientAPI
open TaskManager.Domain.Infrastructure
open TaskManager.Domain.Features.TaskGridView
open TaskManager.Domain.Features.RegisterTask

//type projectionController() =
//    inherit ApiController()
//
//    member x.ReplayProjection(projection:string) = 
//        
//        let retrieveAllEvents =
//            let eventStoreConnectionBuilder = new EventStoreConnectionBuilder()        
//            use connection = eventStoreConnectionBuilder.Build()
//            connection.ConnectAsync().Wait()
//            connection.ReadAllEventsForwardAsync(0, int.MaxValue, false).Result.Events |> aggregateStreams

//        let aggregateStreams (events:List<ResolvedEvent>) =
//            // group into sequences based on OriginalStreamId in ResolvedEvent
//            // deserialize each event
//            // 
//        
//
//        let deserializeEvent event = 
//

//        private Event DeserializeEvent(ResolvedEvent e)
//        {
//            var eventClrTypeName =
//                JObject.Parse(Encoding.UTF8.GetString(e.OriginalEvent.Metadata)).Property(EventClrTypeHeader).Value;
//            string data = Encoding.UTF8.GetString(e.OriginalEvent.Data);
//
//            Type type = Type.GetType((string) eventClrTypeName);
//            var obj = JsonConvert.DeserializeObject(data, type);
//            return (Event) obj;
//        }




//
//        let eventStoreSubscription = connection.SubscribeToStreamFrom(stream, StreamPosition.Start, false, processEvent)
//        eventStoreSubscription.Close
//
//        let eraseProjection documentType =
//            let ravenDbStore = (new RavenDbStore(true, false)).Instance
//            use session = ravenDbStore.OpenSession()
//            let allDocuments = session.Query<documentType>() |> list
//            for document in allDocuments do
//                session.Delete document
//
//        let processTaskRegistered catchUpSubscription event = 
//            let ravenDbStore = (new RavenDbStore(true, false)).Instance
//            use session = ravenDbStore.OpenSession()
//            let taskRegistered = event :?> TaskRegistered
//            let taskInGridView = new TaskInGridView(taskRegistered.TaskId, taskRegistered.ProjectId, taskRegistered.Title, taskRegistered.Deadline, taskRegistered.Priority, false)
//            session.Store taskInGridView
//            session.SaveChanges()


        


//           var eventClrTypeName =
//                JObject.Parse(Encoding.UTF8.GetString(e.OriginalEvent.Metadata)).Property(EventClrTypeHeader).Value;
//            string data = Encoding.UTF8.GetString(e.OriginalEvent.Data);
//
//            Type type = Type.GetType((string) eventClrTypeName);
//            var obj = JsonConvert.DeserializeObject(data, type);
//            return (Event) obj;