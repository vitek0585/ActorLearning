using System;
using Akka.Actor;
using Akka.Cluster;
using Akka.Routing;
using AkkaTextAnalizatorCommon.Messages;
using AkkaTextAnalizatorCommon.Utils;

namespace AkkaTextAnalizatorCommon.Actors
{
    public class SupervisionActor : ReceiveActor
    {
        private int msg = 20;
        protected Akka.Cluster.Cluster Cluster = Akka.Cluster.Cluster.Get(Context.System);

        public SupervisionActor()
        {
            var a = Context.ActorOf(Props.Create<AnalizatorCoordinatorActor>().WithRouter(FromConfig.Instance), "AnalizatorCoordinatorActor");
            Context.Watch(a);
            
            Cluster.Subscribe(a, ClusterEvent.SubscriptionInitialStateMode.InitialStateAsEvents,
            new[]{ typeof(ClusterEvent.IMemberEvent)});

            Receive<Terminated>(r =>
            {
                ColorConsole.WriteLineRed(r.ActorRef.Path + " Terminated");
            });
            //Receive<TextMessage>(m => a.Tell(m,Self));
            Receive<TextMessage>(m =>
            {
                try
                {
                    //Sender.Tell(a.Ask(m, TimeSpan.FromSeconds(10)).Result);
                    a.Forward(m);

                }
                catch (Exception e)
                {
                    ColorConsole.WriteLineRed(e.Message);
                    Sender.Tell(e);
                }

            });

            Receive<int>(id =>
            {
                // Console.WriteLine($"Complete {--msg} id- {id}");
                if (--msg == 0)
                {
                    Console.WriteLine("SupervisionActor Terminate");
                    Context.System.Terminate();
                }
            });

            Receive<int>(id =>
            {
                //Console.WriteLine($"Send {id}");
                Context.Parent.Tell(id);
            });

            //Receive<ClusterEvent.MemberUp>(o => { OnReceiveActorMessage(o); });
            Receive<ClusterEvent.UnreachableMember>(o => { OnReceiveActorMessage(o); });
           Receive<ClusterEvent.IMemberEvent>(o => { OnReceiveActorMessage(o); });
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                exception =>
                {
                    Console.WriteLine("Exception SupervisorStrategy SupervisionActor");
                    if (exception is ArgumentException)
                    {
                        Console.WriteLine("Exception SupervisionActor");
                        return Directive.Resume;
                    }
                    //if (exception is Exception)
                    //{
                    //    return Directive.Resume;
                    //}

                    return Directive.Resume;
                }
                );

        }
        protected override void PreStart()
        {
            ColorConsole.WriteMagenta("SupervisionActor PreStart");
            base.PreStart();
        }

        protected override void PostStop()
        {
            ColorConsole.WriteMagenta("SupervisionActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteMagenta("SupervisionActor PreRestart because: {0}", reason.Message);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteMagenta("SupervisionActor PostRestart because: {0} ", reason.Message);

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