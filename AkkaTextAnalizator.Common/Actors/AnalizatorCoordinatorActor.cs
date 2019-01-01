using System;
using Akka.Actor;
using Akka.Cluster;
using Akka.Routing;
using AkkaTextAnalizatorCommon.Messages;
using AkkaTextAnalizatorCommon.Utils;

namespace AkkaTextAnalizatorCommon.Actors
{
    public class AnalizatorCoordinatorActor : ReceiveActor
    {
        protected Akka.Cluster.Cluster Cluster = Akka.Cluster.Cluster.Get(Context.System);
        private int count = 0;
        public AnalizatorCoordinatorActor()
        {
           // Console.WriteLine("CTOR AnalizatorCoordinatorActor");
            var pool = Context.ActorOf(Props.Create<AnalizeActor>().WithRouter(new RoundRobinPool(3))
            , "Analize");
            Receive<TextMessage>(m =>
            {
                //count=9;
               // Console.WriteLine(GetHashCode());
                //Console.WriteLine($"Received {m.Id} \n{Sender.Path}\n{Context.Parent.Path}\n{Self.Path}");
               // Sender.Tell(pool.Ask(m, TimeSpan.FromSeconds(5)).Result);
                pool.Forward(m);

                //pool.Tell(m);
            });

            Receive<int>(id =>
            {
               // Console.WriteLine($"Coordinator {id} {count}");
               // Console.WriteLine($"Received {id} \n{Sender.Path}\n{Self.Path}\n");
                //Context.Parent.Tell(id);
            });

            //Receive<ClusterEvent.MemberUp>(o => { OnReceiveActorMessage(o); });
            //Receive<ClusterEvent.UnreachableMember>(o => { OnReceiveActorMessage(o); });
            //Receive<ClusterEvent.MemberRemoved>(o => { OnReceiveActorMessage(o); });
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                exception =>
                {
                    if (exception is ArgumentException)
                    {
                        ColorConsole.WriteLineRed("Exception AnalizatorCoordinatorActor");
                        return Directive.Escalate;
                    }
                    //if (exception is Exception)
                    //{
                    //    return Directive.Resume;
                    //}

                    return Directive.Restart;
                }
                );

        }
        
        protected override void PreStart()
        {
            //Cluster.Subscribe(Self, ClusterEvent.InitialStateAsEvents,
            //    new[] { typeof(ClusterEvent.IMemberEvent), typeof(ClusterEvent.UnreachableMember) });
            ColorConsole.WriteMagenta("AnalizatorCoordinatorActor PreStart");
            base.PreStart();
        }

        protected override void PostStop()
        {
            // Cluster.Unsubscribe(Self);
            ColorConsole.WriteMagenta("AnalizatorCoordinatorActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteMagenta("AnalizatorCoordinatorActor PreRestart because: {0}", reason.Message);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteMagenta("AnalizatorCoordinatorActor PostRestart because: {0} ", reason.Message);

            base.PostRestart(reason);
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