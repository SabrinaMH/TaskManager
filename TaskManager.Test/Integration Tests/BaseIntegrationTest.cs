using NUnit.Framework;
using Ploeh.AutoFixture;
using Raven.Client;
using TaskManager.Domain.Infrastructure;

namespace TaskManager.Test
{
    [TestFixture]
    public class BaseIntegrationTest
    {
        protected IDocumentStore DocumentStore;
        public static CommandDispatcher CommandDispatcher;
        public static EventBus EventBus;
        protected IEventStoreConnectionBuilder InMemoryEventStoreConnectionBuilder;
        protected Fixture Fixture;

        [SetUp]
        public void BaseSetUp()
        {
            DocumentStore = new RavenDbStore(true, false).Instance;
            InMemoryEventStoreConnectionBuilder = new InMemoryEventStoreConnectionBuilder();
            EventBus = new EventBus(null);
            CommandDispatcher = new CommandDispatcher(InMemoryEventStoreConnectionBuilder, EventBus);
            Fixture = new Fixture();
        }

        [TearDown]
        public void BaseTearDown()
        {
            DocumentStore.Dispose();
        }
    }
}