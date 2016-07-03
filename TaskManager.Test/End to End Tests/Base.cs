using MediatR;
using Ploeh.AutoFixture;
using Raven.Client;
using TaskManager.Domain.Infrastructure;
using TechTalk.SpecFlow;

namespace TaskManager.Test
{
    [Binding]
    public class Base
    {
        public static IDocumentStore DocumentStore;
        public static IMediator Mediator;
        public static IEventStoreConnectionBuilder InMemoryEventStoreConnectionBuilder;
        public static Fixture Fixture;

        [BeforeTestRun]
        public static void BaseSetUp()
        {
            DocumentStore = new RavenDbStore(true, false).Instance;
            InMemoryEventStoreConnectionBuilder = new InMemoryEventStoreConnectionBuilder();
            Mediator = new Mediate(InMemoryEventStoreConnectionBuilder).Mediator;
            Fixture = new Fixture();
        }

        [AfterTestRun]
        public static void BaseTearDown()
        {
            DocumentStore.Dispose();
        }
    }
}