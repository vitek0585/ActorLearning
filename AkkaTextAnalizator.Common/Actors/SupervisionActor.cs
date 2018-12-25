using System;
using Akka.Actor;
using Akka.Routing;

namespace AkkaTextAnalizatorCommon.Actors
{
    public class SupervisionActor : ReceiveActor
    {
        private int msg = 20;
        public SupervisionActor()
        {
            Context.ActorOf(Props.Create<AnalizatorCoordinatorActor>().WithRouter(FromConfig.Instance), "AnalizatorCoordinatorActor");
            //Context.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "AnalizatorCoordinatorActor");
            //Context.ActorOf(Props.Create<AnalizatorCoordinatorActor>(), "AnalizatorCoordinatorActor");
            //Receive<int>(id =>
            //{
            //   // Console.WriteLine($"Complete {--msg} id- {id}");
            //    if (--msg == 0)
            //    {
            //        Console.WriteLine("SupervisionActor Terminate");
            //        Context.System.Terminate();
            //    }
            //});

            //Receive<int>(id =>
            //{
            //    Console.WriteLine($"Send {id}");
            //    Context.Parent.Tell(id);
            //});
        }


        protected override void PreStart()
        {
            Console.WriteLine("PreStart SupervisionActor");
            base.PreStart();
        }
    }
}