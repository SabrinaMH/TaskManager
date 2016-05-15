using System;
using System.IO;
using System.Net.Http;
using Raven.Abstractions.Util;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Linq.Indexing;
using Raven.Client.Listeners;
using Raven.Database.Server;
using Raven.Json.Linq;

namespace TaskManager.Domain.Infrastructure
{
    public class RavenDbStore
    {
        private static IDocumentStore _documentStore;
        private readonly EmbeddableDocumentStore _embeddableDocumentStore;

        public RavenDbStore(bool runInMemory = false, bool allowStaleQueries = true)
        {
            string dataDirectory = ConfigurationManager.GetAppSetting("ravendb.data.directory");
            _embeddableDocumentStore = new EmbeddableDocumentStore
            {
                RunInMemory = runInMemory,
                UseEmbeddedHttpServer = true,
                DataDirectory =dataDirectory
            };

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

    }
}