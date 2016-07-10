using System.Net.Http;
using Raven.Abstractions.Util;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Listeners;

namespace TaskManager.Domain.Infrastructure
{
    public class RavenDbStore
    {
        private static IDocumentStore _documentStore;
        private readonly EmbeddableDocumentStore _embeddableDocumentStore;

        public RavenDbStore(bool runInMemory = false, bool allowStaleQueries = true)
        {
            string dataDirectory = ConfigurationManager.GetAppSetting("ravendb.data.directory");
            int port = int.Parse(ConfigurationManager.GetAppSetting("ravendb.server.port"));

            if (runInMemory)
            {
                _embeddableDocumentStore = new EmbeddableDocumentStore
                {
                    RunInMemory = true,
                    UseEmbeddedHttpServer = true,
                    DataDirectory = dataDirectory,
                    Configuration = {Port = port}
                };
                // Necessary to run tests in the FSharp project.
                _embeddableDocumentStore.Configuration.Storage.Voron.AllowOn32Bits = true;
            }
            else
            {
                _embeddableDocumentStore = new EmbeddableDocumentStore
                {
                    RunInMemory = false,
                    UseEmbeddedHttpServer = false,
                    DataDirectory = dataDirectory,
                    Configuration = {Port = port}
                };
            }

            if (!allowStaleQueries)
            {
                _embeddableDocumentStore.RegisterListener(new NoStaleQueriesAllowedListener());
            }
        }

        public IDocumentStore Instance
        {
            get
            {
                if (_documentStore == null || _documentStore.WasDisposed)
                {
                    _documentStore = _embeddableDocumentStore.Initialize();
                    _embeddableDocumentStore.JsonRequestFactory.ConfigureRequest += (sender, args) =>
                    {
                        var webRequestHandler = new WebRequestHandler();
                        webRequestHandler.UnsafeAuthenticatedConnectionSharing = true;
                        webRequestHandler.PreAuthenticate = true;
                        args.Client = new HttpClient(webRequestHandler);
                    };
                }
                return _documentStore;
            }
        }

        private class NoStaleQueriesAllowedListener : IDocumentQueryListener
        {
            public void BeforeQueryExecuted(IDocumentQueryCustomization queryCustomization)
            {
                queryCustomization.WaitForNonStaleResults();
            }
        }

        public static void CleanUp()
        {
            if (_documentStore == null) return;

            _documentStore.Dispose();
        }
    }
}