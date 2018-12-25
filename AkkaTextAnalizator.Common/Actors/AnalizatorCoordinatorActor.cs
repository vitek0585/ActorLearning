using System;
using Akka.Actor;
using Akka.Cluster;
using Akka.Routing;
using AkkaTextAnalizatorCommon.Messages;

namespace AkkaTextAnalizatorCommon.Actors
{
    public class AnalizatorCoordinatorActor : ReceiveActor
    {
        protected Akka.Cluster.Cluster Cluster = Akka.Cluster.Cluster.Get(Context.System);

        public AnalizatorCoordinatorActor()
        {
            Console.WriteLine("CTOR AnalizatorCoordinatorActor");
            var pool = Context.ActorOf(Props.Create<AnalizeActor>().WithRouter(new RoundRobinPool(10)), "Analize");
            Receive<TextMessage>(m =>
            {
                Console.WriteLine($"Received {m.Id} \n{Sender.Path}\n{Context.Parent.Path}\n");
                //pool.Tell(m, Sender);
                pool.Tell(m);
            });

            Receive<int>(id =>
            {
                Console.WriteLine($"Coordinator {id}");
                //Console.WriteLine($"Received {id} \n{Sender.Path}\n{Context.Parent.Path}\n");
                //Context.Parent.Tell(id);
            });

            Receive<ClusterEvent.MemberUp>(o => { OnReceiveActorMessage(o); });
            Receive<ClusterEvent.UnreachableMember>(o => { OnReceiveActorMessage(o); });
            Receive<ClusterEvent.MemberRemoved>(o => { OnReceiveActorMessage(o); });
        }

        protected override void PreStart()
        {
            Cluster.Subscribe(Self, ClusterEvent.InitialStateAsEvents,
                new[] { typeof(ClusterEvent.IMemberEvent), typeof(ClusterEvent.UnreachableMember) });
            Console.WriteLine("Pre Start AnalizatorCoordinatorActor");
            base.PreStart();
        }

        protected override void PostStop()
        {
            Cluster.Unsubscribe(Self);
            Console.WriteLine("Post Stop AnalizatorCoordinatorActor");
        }

        protected void OnReceiveActorMessage(object message)
        {
            var up = message as ClusterEvent.MemberUp;
            if (up != null)
            {
                var mem = up;
                Console.WriteLine("Member is Up: {0}", mem.Member);
            }
            else if (message is ClusterEvent.UnreachableMember)
            {
                var unreachable = (ClusterEvent.UnreachableMember)message;
                Console.WriteLine("Member detected as unreachable: {0}", unreachable.Member);
            }
            else if (message is ClusterEvent.MemberRemoved)
            {
                var removed = (ClusterEvent.MemberRemoved)message;
                Console.WriteLine("Member is Removed: {0}", removed.Member);
            }
        }
    }
}