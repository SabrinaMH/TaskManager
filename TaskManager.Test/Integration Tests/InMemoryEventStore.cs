using System.Diagnostics;
using System.Net;
using System.Threading;
using EventStore.ClientAPI.Embedded;
using EventStore.ClientAPI.Exceptions;
using EventStore.Core;
using EventStore.Core.Data;

namespace TaskManager.Test
{
    public class InMemoryEventStore
    {
        private static ClusterVNode _node;

        public ClusterVNode Instance
        {
            get
            {
                if (_node == null)
                {
                    Bootstrap();
                }
                return _node;
            }
        }

        private void Bootstrap()
        {
            // GetEventStore runs on the client on default tcp port 1113. Thus, the in memory cannot run on the same.
            _node =
                EmbeddedVNodeBuilder.AsSingleNode()
                    .RunInMemory()
                    .WithInternalTcpOn(new IPEndPoint(IPAddress.Loopback, 1114))
                    .WithExternalTcpOn(new IPEndPoint(IPAddress.Loopback, 1115))
                    .WithInternalHttpOn(new IPEndPoint(IPAddress.Loopback, 2114))
                    .WithExternalHttpOn(new IPEndPoint(IPAddress.Loopback, 2115))
                    .Build();

            bool isNodeMaster = false;
            _node.NodeStatusChanged += (sender, args) => isNodeMaster = args.NewVNodeState == VNodeState.Master;
            _node.Start();

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
        }
    }
}