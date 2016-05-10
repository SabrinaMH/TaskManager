using NUnit.Framework;
using Raven.Client;
using TaskManager.Domain.Infrastructure;

namespace TaskManager.Test
{
    [TestFixture]
    public class BaseIntegrationTest
    {
        protected IDocumentStore DocumentStore;

        [SetUp]
        public void SetUp()
        {
            DocumentStore = new RavenDbStore().Instance;
        }

        [TearDown]
        public void TearDown()
        {
            DocumentStore.Dispose();
        }
    }
}