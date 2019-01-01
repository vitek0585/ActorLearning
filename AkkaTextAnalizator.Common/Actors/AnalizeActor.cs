using System;
using System.Threading;
using Akka.Actor;
using AkkaTextAnalizatorCommon.Messages;
using AkkaTextAnalizatorCommon.Utils;

namespace AkkaTextAnalizatorCommon.Actors
{
    public class AnalizeActor : ReceiveActor
    {
        public AnalizeActor()
        {
            Receive<TextMessage>(m =>
            {
                HandleMessage(m);
                //Become(BecomeNewBehave); change state only one actor
            });
        }
        
        private void BecomeNewBehave()
        {
            Receive<TextMessage>(m =>
            {
                Console.WriteLine("Become");
            });
        }

        private void HandleMessage(TextMessage message)
        {
            try
            {

                //AnalizatorEngine.CalculateTotalSpaces(message.Message, message.Id);
                //Console.WriteLine($"HandleMessage \n{Sender.Path}\n{Context.Parent.Path}\n");
                ColorConsole.WriteLineYellow(Thread.CurrentThread.ManagedThreadId+" "+message.Id);
                Thread.Sleep(2000);
                Sender.Tell(message.Id);
                //if(message.Id == 9990)
                //Self.Tell(PoisonPill.Instance);
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }


        #region Lifecycle hooks

        protected override void PreStart()
        {
            ColorConsole.WriteMagenta("AnalizeActor PreStart");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteMagenta("AnalizeActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteMagenta("AnalizeActor PreRestart because: {0}", reason.Message);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteMagenta("AnalizeActor PostRestart because: {0} ", reason.Message);

            base.PostRestart(reason);
        }
        #endregion

    }
}