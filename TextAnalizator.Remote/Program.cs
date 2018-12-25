using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaTextAnalizatorCommon.Actors;

namespace TextAnalizator.Remote
{
    class Program
    {
        private static ActorSystem AnalizeTextActorSystem;
        static void Main(string[] args)
        {
            Console.WriteLine("Creating AnalizeTextActorSystem in remote process");

            AnalizeTextActorSystem = ActorSystem.Create("AnalazerActorSystem");
           // AnalizeTextActorSystem.ActorOf(Props.Create<SupervisionActor>(), "SupervisionActor");
            Console.ReadLine();
            AnalizeTextActorSystem.Terminate().Wait();
        }
    }
}
