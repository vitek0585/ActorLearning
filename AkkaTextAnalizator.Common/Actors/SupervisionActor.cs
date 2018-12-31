using System;
using Akka.Actor;
using Akka.Routing;
using AkkaTextAnalizatorCommon.Messages;
using AkkaTextAnalizatorCommon.Utils;

namespace AkkaTextAnalizatorCommon.Actors
{
    public class SupervisionActor : ReceiveActor
    {
        private int msg = 20;
        public SupervisionActor()
        {
            var a = Context.ActorOf(Props.Create<AnalizatorCoordinatorActor>().WithRouter(FromConfig.Instance), "AnalizatorCoordinatorActor");

            //Receive<TextMessage>(m => a.Tell(m,Self));
            Receive<TextMessage>(m =>
            {
                try
                {
                throw new Exception("1234");
                    Sender.Tell(a.Ask(m, TimeSpan.FromSeconds(10)).Result);

                }
                catch (Exception e)
                {
                    ColorConsole.WriteLineRed(e.Message);
                    Sender.Tell(e);
                }

            });

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

            Receive<int>(id =>
            {
                //Console.WriteLine($"Send {id}");
                Context.Parent.Tell(id);
            });
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
                        return Directive.Stop;
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
    }
}