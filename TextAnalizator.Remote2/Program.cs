using System;
using Akka.Actor;
using AkkaTextAnalizatorCommon.Actors;

namespace TextAnalizator.Remote2
{
    class Program
    {
        private static ActorSystem AnalizeTextActorSystem;
        static void Main(string[] args)
        {
            Console.WriteLine("Creating AnalizeTextActorSystem 2 in remote process");

            AnalizeTextActorSystem = ActorSystem.Create("AnalazerActorSystem");
            AnalizeTextActorSystem.ActorOf(Props.Create<SupervisionActor>(), "SupervisionActor");
            Console.ReadLine();
            AnalizeTextActorSystem.Terminate().Wait();
        }
    }
}
