using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using AkkaTextAnalizatorCommon.Actors;
using AkkaTextAnalizatorCommon.Utils;

namespace AkkaTextAnalizator
{
    class Program
    {

        static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("AnalazerActorSystem");
           // Thread.Sleep(3000);
            var sp = actorSystem.ActorOf(Props.Create<SupervisionActor>(), "SupervisionActor");
            
            //var analizatorCoordinatorActor = actorSystem.ActorOf(Props.Create<AnalizatorCoordinatorActor>(), "AnalizatorCoordinatorActor");
            
            var startNew = Stopwatch.StartNew();
            //var group = actorSystem.ActorOf(Props.Create<GroupActor>(), "GroupActor");
            startNew.Start();
            //group.Tell("start");

            foreach (var textMessage in new TextMessageStorage().Get(200))
            {
                Console.ReadKey();
                Thread.Sleep(1000);
                actorSystem.ActorSelection("/user/SupervisionActor/AnalizatorCoordinatorActor").Tell(textMessage);
                //sp.Tell(textMessage);
            }

            actorSystem.WhenTerminated.Wait();
            startNew.Stop();
            Console.WriteLine($"{startNew.ElapsedMilliseconds}");
            Console.ReadKey();
        }


    }
}
