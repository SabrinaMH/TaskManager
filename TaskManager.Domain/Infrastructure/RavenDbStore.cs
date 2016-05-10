using System;
using System.IO;
using System.Net.Http;
using Raven.Abstractions.Util;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Linq.Indexing;
using Raven.Database.Server;

namespace TaskManager.Domain.Infrastructure
{
    public class RavenDbStore
    {
        private static IDocumentStore _documentStore;
        private readonly EmbeddableDocumentStore _embeddableDocumentStore;

        public RavenDbStore()
        {
            string dataDirectory = ConfigurationManager.GetAppSetting("ravendb.data.directory");
            _embeddableDocumentStore = new EmbeddableDocumentStore
            {
                RunInMemory = true,
                UseEmbeddedHttpServer = true,
                DataDirectory = @"c:\temp\ravendb",
            };

            //int port = 8080;
            //_embeddableDocumentStore.Configuration.Port = port;
            //_embeddableDocumentStore.Configuration.HostName = "localhost";
            _embeddableDocumentStore.Configuration.AnonymousUserAccessMode = AnonymousUserAccessMode.Admin;
            //NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(port);
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
    }
}