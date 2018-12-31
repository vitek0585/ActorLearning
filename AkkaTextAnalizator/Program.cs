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
using Akka.Util.Internal;
using AkkaTextAnalizatorCommon.Actors;
using AkkaTextAnalizatorCommon.Messages;
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

            //var result1 = sp.Ask<int>(new TextMessage("", 3)).Result;
            //Console.WriteLine($"Get result {result1}");
            //if (result1 != 3)
            //{
            //    Console.WriteLine($"Error {result1}");
            //}

            //group.Tell("start");
            startNew.Start();

            new TextMessageStorage().Get(10000).ForEach(i =>
            {
                Console.WriteLine("Read key");
                Console.ReadKey();
                try
                {
                    var result = sp.Ask<int>(i).Result;
                    if (result != i.Id)
                    {
                        Console.WriteLine($"Error {result}");
                    }
                    else if (i.Id < 5) { Console.WriteLine($"Starting {result}"); }
                }
                catch (Exception e)
                {
                    ColorConsole.WriteLineGreen("Main " + e.Message);
                    throw;
                }
                 //sp.Tell(new TextMessage("", i));


                 //actorSystem.ActorSelection("/user/SupervisionActor/AnalizatorCoordinatorActor").Ask(new TextMessage("", i));
             });

            startNew.Stop();
            Console.WriteLine($"{startNew.ElapsedMilliseconds}");
            Console.WriteLine("Finish------------------------------");
            actorSystem.WhenTerminated.Wait();

            Console.ReadKey();
        }


    }
}
