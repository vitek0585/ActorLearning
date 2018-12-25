using System;
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
            });
        }

        private void HandleMessage(TextMessage message)
        {
            AnalizatorEngine.CalculateTotalSpaces(message.Message, message.Id);
            //Console.WriteLine($"HandleMessage \n{Sender.Path}\n{Context.Parent.Path}\n");
            Sender.Tell(message.Id);
        }
    }
}