using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using Couchbase;
using NUnit.Framework;
using Ploeh.AutoFixture;
using TaskManager.Domain.Features.ViewProjectTree;
using TaskManager.Domain.Infrastructure;

namespace TaskManager.Test.ViewProjectTreeTest
{
    [TestFixture]
    public class ProjectTreeViewQueryHandlerTest
    {
        CouchbaseRestClient _couchbaseRestClient;
        string _bucketName;

        [SetUp]
        public void SetUp()
        {
            _couchbaseRestClient = new CouchbaseRestClient();
            _bucketName = ConfigurationManager.AppSettings.Get("couchbase.bucket.projectTreeNode");
                _couchbaseRestClient.FlushBucket(_bucketName);        
        }

        [Test]
        public void Can_Retrieve_All_Projects()
        {
            var fixture = new Fixture();

            var cluster = new Cluster();
            using (var bucket = cluster.OpenBucket(_bucketName))
            {
                string id = fixture.Create<Guid>().ToString();
                var document = new Document<ProjectTreeNode>
                {
                    Id = id,
                    Content =
                        new ProjectTreeNode(id, fixture.Create<string>(), fixture.Create<DateTime>().ToShortDateString())
                };

                bucket.Insert(document);
            }

            var projectTreeViewQueryHandler = new ProjectTreeViewQueryHandler();
            var allProjectTreeNodesQuery = new AllProjectTreeNodesQuery();
            List<ProjectTreeNode> projectTreeNodes = projectTreeViewQueryHandler.Handle(allProjectTreeNodesQuery);
            Assert.That(projectTreeNodes.Count, Is.EqualTo(1));
        }

        [TearDown]
        public void TearDown()
        {
            _couchbaseRestClient.FlushBucket(_bucketName);
            _couchbaseRestClient.Dispose();
        }
    }
}