using System.Diagnostics;
using System.Net;
using System.Threading;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.ClientAPI.Exceptions;
using EventStore.Core;
using EventStore.Core.Data;
using NUnit.Framework;

namespace TaskManager.Test
{
    [SetUpFixture]
    public class InMemoryEventStore
    {
        public static IEventStoreConnection Connection { get; private set; }
        
        [SetUp]
        public static void SetupInMemoryEventStore()
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
            Connection = eventStoreConnection;
        }

        [TearDown]
        public static void TeardownInMemoryEventStore()
        {
            Connection.Close();
            Connection.Dispose();
        }
    }
}