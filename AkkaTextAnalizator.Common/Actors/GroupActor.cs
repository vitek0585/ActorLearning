using System;
using System.Linq;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using AkkaTextAnalizatorCommon.Utils;

namespace AkkaTextAnalizatorCommon.Actors
{
    public class GroupActor : ReceiveActor
    {
        private int messageCount = 100;

        public GroupActor()
        {
            
        }

        private void InitializeRemoteGroup()
        {
            string[] s = {
            "akka.tcp://AnalazerActorSystem@127.0.0.1:8091/user/SupervisionActor/AnalizatorCoordinatorActor",
            "akka.tcp://AnalazerActorSystem@127.0.0.1:8092/user/SupervisionActor/AnalizatorCoordinatorActor",
            "akka.tcp://AnalazerActorSystem@127.0.0.1:8093/user/SupervisionActor/AnalizatorCoordinatorActor"
            };

            var group = Context.ActorOf(Props.Empty.WithRouter(new RoundRobinGroup(s)), "my-group");

            Receive<int>(id =>
            {
                Console.WriteLine($"Complete {messageCount} id- {id}");
                if (--messageCount == 0)
                {
                    Console.WriteLine("SupervisionActor Terminate");
                    Context.System.Terminate();
                }
            });

            var storage = new TextMessageStorage();
            var textMessages = storage.Get(messageCount);

            Receive<string>(m =>
            {
                Console.WriteLine("Start");
                foreach (var textMessage in textMessages)
                {
                    group.Tell(textMessage);
                }
            });
        }
    }
}